using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.Utils;

namespace GpuJobs.OpenCl.Core {
	public class ClContext : ClHandle {
		public ClDevice[] devices;
		public ClContext(IntPtr handle) : base(handle) { }
		public ClContext(IntPtr handle, ClDevice[] devices) : base(handle) => this.devices = devices;
		protected override void ReleaseUnmanagedResources() { ClContextApi.ClReleaseContext(handle); }
		
		public unsafe void* CreateBuffer(ClMemFlags memoryFlags, int size, void* hostPtr, bool useHostPtr) {
			if (hostPtr == null) return CreateBuffer(memoryFlags, (UIntPtr) size, null);
			
			memoryFlags |= useHostPtr ? ClMemFlags.CL_MEM_USE_HOST_PTR : ClMemFlags.CL_MEM_COPY_HOST_PTR;
			return CreateBuffer(memoryFlags, (UIntPtr) size, hostPtr);
		}
		
		public unsafe void* CreateBuffer(ClMemFlags memoryFlags, UIntPtr size, void* hostPtr) {
			ClResult* result = stackalloc ClResult[1];
			void* ptr = ClMemApi.ClCreateBuffer(handle, memoryFlags, size, hostPtr, result);
			result->CheckResult("CreateBuffer error");
			return ptr;
		}

		public unsafe void* CreateBuffer<T>(ClMemFlags flags, int length, T* hostPtr, bool useHostPtr) where T : unmanaged {
			return CreateBuffer(flags, length * sizeof(T), (void*) hostPtr, useHostPtr);
		}
		
		public unsafe void* CreateBuffer<T>(ClMemFlags flags, T[] src, bool useHostPtr) where T : unmanaged {
			fixed (T* hostPtr = src) return CreateBuffer(flags, src.Length, hostPtr, useHostPtr);
		}
		
		public byte[] GetProgramBuildInfo(IntPtr progHandle, ClDevice device, ClProgramBuildInfo paramName) {
			ClProgramApi.ClGetProgramBuildInfo(progHandle, device.handle, paramName, UIntPtr.Zero, null, out UIntPtr valueSize)
						.CheckResult("GetProgramBuildInfo error");

			byte[] output = new byte[(int) valueSize];
			ClProgramApi.ClGetProgramBuildInfo(progHandle, device.handle, paramName, new((uint) output.Length), output, out _)
						.CheckResult("GetProgramBuildInfo error");
			return output;
		}

		public string GetProgramBuildLog(IntPtr progHandle, ClDevice device) => GetProgramBuildInfo(progHandle, device, ClProgramBuildInfo.CL_PROGRAM_BUILD_LOG).AsString();

		public ClProgram BuildProgramFromPath(string path) {
			using FileStream fs = new(path, FileMode.Open);
			using StreamReader sr = new(fs);
			return BuildProgramFromString(sr.ReadToEnd());
		}
		
		public ClProgram BuildProgramFromString(string src) => BuildProgramFromString(new[] {src});
		
		public ClProgram BuildProgramFromString(string[] src) {
			UIntPtr[] srcLengths = src.Select(str => (UIntPtr) str.Length).ToArray();
			IntPtr programPtr = ClProgramApi.ClCreateProgramWithSource(handle, 1, src, srcLengths, out ClResult result);
			result.CheckResult($"BuildProgramFromString.src error");

			BuildProgramCallback callback = (program, _) => {
				int deviceCount = devices.Length;
				string[] logs = new string[deviceCount];
				int logCount = 0;
				
				for (int i = 0; i < deviceCount; i++) {
					string log = GetProgramBuildLog(program, devices[i]);
					logs[i] = log;
					if (!string.IsNullOrWhiteSpace(log)) logCount++;
				}

				programPtr = program;
				if (logCount == 0) return;
				string allLogs = "";
				for (int i = 0; i < deviceCount; i++) {
					if (string.IsNullOrEmpty(logs[i])) continue;
					allLogs += $"device #{i}:\n{logs[i]}\n";
				}

				throw new ClException($"program build error", ClResult.CL_BUILD_PROGRAM_FAILURE, allLogs);
			};
			
			ClProgramApi.ClBuildProgram(programPtr, 0, null, null, Marshal.GetFunctionPointerForDelegate(callback), IntPtr.Zero).CheckResult($"BuildProgramFromString.build error");
			return new(programPtr);
		}
		
		private delegate void BuildProgramCallback(IntPtr program, IntPtr userData);
		
		public static ClContext CreateContext(ClDevice devices) => CreateContext(new[] {devices});
			
		public static ClContext CreateContext(ClDevice[] devices) {
			IntPtr ptr = ClContextApi.ClCreateContext(IntPtr.Zero, (uint) devices.Length, devices.Select(device => device.handle).ToArray(), IntPtr.Zero, IntPtr.Zero, out ClResult result);
			result.CheckResult("CreateContext error");
			return new(ptr, devices);
		}
	}
}