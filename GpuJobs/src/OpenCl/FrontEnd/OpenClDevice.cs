using System;
using System.Collections.Generic;
using GpuJobs.OpenCl.Core;

namespace GpuJobs.OpenCl.FrontEnd {
	public class OpenClDevice : IDisposable {
		public int id;
		public ClDevice device;
		public ClContext context;
		public ClCommandQueue cmdQueue;
		public bool isFree;

		protected Dictionary<string, ClProgram> compiledPrograms = new();
		protected Dictionary<string, ClKernel> compiledKernels = new();

		public OpenClDevice() { }

		public OpenClDevice(int id, ClDevice device, ClContext context, ClCommandQueue cmdQueue) {
			this.id = id;
			this.device = device;
			this.context = context;
			this.cmdQueue = cmdQueue;
		}
		
		public bool TryAddKernelsFromPath(string path, string progName, string[] kernelNames) {
			if (compiledPrograms.ContainsKey(progName)) return false;
			AddKernelsFromPath(path, progName, kernelNames);
			return true;
		}

		public void AddKernelsFromPath(string path, string progName, string[] kernelNames) {
			ClProgram prog = context.BuildProgramFromPath(path);
			foreach (string s in kernelNames) {
				compiledKernels.Add($"{progName}:{s}", prog.CreateKernel(s));
			}
			compiledPrograms.Add(progName, prog);
		}
		
		public bool TryAddKernelsFromString(string src, string progName, string[] kernelNames) {
			if (compiledPrograms.ContainsKey(progName)) return false;
			AddKernelsFromString(src, progName, kernelNames);
			return true;
		}
		
		public void AddKernelsFromString(string src, string progName, string[] kernelNames) {
			ClProgram prog = context.BuildProgramFromString(src);
			foreach (string s in kernelNames) {
				compiledKernels.Add($"{progName}:{s}", prog.CreateKernel(s));
			}
			compiledPrograms.Add(progName, prog);
		}

		public bool ContainsKernel(string name) => compiledKernels.ContainsKey(name);
		public bool ContainsKernel(string programName, string name) => compiledKernels.ContainsKey($"{programName}:{name}");
		public bool ContainsProgram(string name) => compiledPrograms.ContainsKey(name);

		protected ClKernel GetKernel(string name) => compiledKernels[name];
		public ClKernel GetKernel(string programName, string name) => compiledKernels[$"{programName}:{name}"];

		public void Free() {
			if (isFree) return;
			OpenClCompute.availableDevices.Enqueue(id);
			isFree = true;
		}

		public void Dispose() {
			device?.Dispose();
			context?.Dispose();
			cmdQueue?.Dispose();
		}

		public override string ToString() => $"#{id}: {device}";
	}
}