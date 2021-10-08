using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.Core;
using GpuJobs.OpenCl.FrontEnd;

namespace GpuJobs.Jobs {
	[Flags]
	public enum GpuJobArgFlags : byte {
		Read = 1,
		Write = 2,
		ReadWrite = Read | Write,
		Local = 4,
		AutoMemManagement = 8,
	}
	
	public struct GpuJobArg {
		public FieldInfo field;
		public GpuJobArgFlags flags;
		public int argId;

		public GpuJobArg(FieldInfo field, GpuJobArgFlags flags, int argId) {
			this.field = field;
			this.flags = flags;
			this.argId = argId;
		}
	}
	
	public abstract class GpuJobBase : IDisposable {
		private static Dictionary<Type, GpuJobArg[]> _args = new();
		
		public OpenClDevice device;
		public ClKernel kernel;

		protected abstract string progName { get; }
		protected abstract string kernelName { get; }
		protected abstract string programStr { get; }
		protected abstract string[] programKernels { get; }
		protected virtual bool usePathInsteadOfSrc => false;
		protected bool buffersAllocated = false;
		
		protected GpuJobBase() {
			device = OpenClCompute.WaitForAvailableDeviceAndPop();
			kernel = GetKernel();
		}

		private ClKernel GetKernel() {
			if (!device.ContainsProgram(progName)) {
				if (usePathInsteadOfSrc) device.TryAddKernelsFromPath(programStr, progName, programKernels);
				else device.TryAddKernelsFromString(programStr, progName, programKernels);
			}
			return device.GetKernel(progName, kernelName);
		}

		public Task RunAsync(int iterations) => Task.Run(() => Run(iterations));
		
		public void Run(int iterations) {
			ApplyKernelArgs();
			EnqueueKernel(iterations);
			ReadArgs();
			device.Free();
		}

		private void EnqueueKernel(int iterations) {
			kernel.Run(device.cmdQueue, iterations);
		}

		private void ApplyKernelArgs() {
			GpuJobArg[] args = GetArgs();
			foreach (GpuJobArg arg in args) {
				object argValue = arg.field.GetValue(this);
				if (argValue is ClBuffer buffer) {
					buffer.device = device;
					
					if ((arg.flags & GpuJobArgFlags.Read) != 0) buffer.flags |= ClMemFlags.CL_MEM_READ_ONLY;
					if ((arg.flags & GpuJobArgFlags.Write) != 0) buffer.flags |= ClMemFlags.CL_MEM_WRITE_ONLY;
					if ((arg.flags & GpuJobArgFlags.ReadWrite) != 0) {
						buffer.flags &= ~ClMemFlags.CL_MEM_READ_ONLY;
						buffer.flags &= ~ClMemFlags.CL_MEM_WRITE_ONLY;
						buffer.flags |= ClMemFlags.CL_MEM_READ_WRITE;
					}
					if ((arg.flags & GpuJobArgFlags.AutoMemManagement) != 0 && !buffersAllocated) buffer.CopyToDevice();
					kernel.SetKernelArg(arg.argId, buffer.handle);
					continue;
				}

				if (argValue is ClMemObject obj) {
					kernel.SetKernelArg(arg.argId, obj.handle);
					continue;
				}

				throw new NotSupportedException($"GpuJobs support only ClMemObject objects as arguments");
			}

			buffersAllocated = true;
		}

		private GpuJobArg[] GetArgs() {
			Type t = GetType();
			return _args.TryGetValue(t, out GpuJobArg[] existingArgs) ? existingArgs : AddArgs();
		}

		private GpuJobArg[] AddArgs() {
			Type t = GetType();
			List<GpuJobArg> args = new();

			FieldInfo[] fields = t.GetFields();
			foreach (FieldInfo field in fields) {
				ClReadAttribute rAttr = field.GetCustomAttribute<ClReadAttribute>();
				ClWriteAttribute wAttr = field.GetCustomAttribute<ClWriteAttribute>();
				ClReadWriteAttribute rwAttr = field.GetCustomAttribute<ClReadWriteAttribute>();

				if (rAttr != null) {
					GpuJobArgFlags flags = GpuJobArgFlags.Read;
					if (rAttr.autoMemManagement) flags |= GpuJobArgFlags.AutoMemManagement;
					GpuJobArg arg = new(field, flags, rAttr.argId);
					args.Add(arg);
					continue;
				}
				if (wAttr != null) {
					GpuJobArgFlags flags = GpuJobArgFlags.Write;
					if (wAttr.autoMemManagement) flags |= GpuJobArgFlags.AutoMemManagement;
					GpuJobArg arg = new(field, flags, wAttr.argId);
					args.Add(arg);
					continue;
				}
				if (rwAttr != null) {
					GpuJobArgFlags flags = GpuJobArgFlags.ReadWrite;
					if (rwAttr.autoMemManagement) flags |= GpuJobArgFlags.AutoMemManagement;
					GpuJobArg arg = new(field, flags, rwAttr.argId);
					args.Add(arg);
				}
			}

			GpuJobArg[] argsArr = args.ToArray();
			_args.Add(t, argsArr);
			return argsArr;
		}

		private void ReadArgs() {
			GpuJobArg[] args = GetArgs();
			foreach (GpuJobArg arg in args) {
				if ((arg.flags & GpuJobArgFlags.AutoMemManagement) == 0 || (arg.flags & GpuJobArgFlags.Write) == 0) continue;
				object argValue = arg.field.GetValue(this);
				if (argValue is ClBuffer buffer) {
					buffer.CopyFromDevice();
					continue;
				}

				if (argValue is ClMemObject obj) {
					throw new NotSupportedException($"GpuJobs doesn't support ClMemObject read/read-write objects yet, use ClBuffer");
				}
				
				throw new NotSupportedException($"GpuJobs support only ClBuffer objects for read/read-write");
			}
		}

		public void Dispose() {
			GpuJobArg[] args = GetArgs();
			foreach (GpuJobArg arg in args) {
				if ((arg.flags & GpuJobArgFlags.AutoMemManagement) == 0) continue;
				object argValue = arg.field.GetValue(this);
				if (argValue == null) continue;
				((IDisposable) argValue).Dispose();
			}
		}
	}
}