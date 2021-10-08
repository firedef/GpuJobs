using System;
using GpuJobs.Jobs;
using GpuJobs.OpenCl.Core;

namespace GpuJobs.tests.jobs {
	public static class OpenClTest_1 {
		public static void Run() {
			TestGpuJob job = new();
			int iterationCount = 1024;
			
			// you must allocate all buffers before run
			job.dest = job.device.AllocateOnHost<byte>(iterationCount);
			job.src = job.device.AllocateOnHost<byte>(iterationCount);
			
			// fill buffer
			// you can use job.src.hostPtr[i] for faster access and MemManager.MemCpy for faster copy
			for (int i = 0; i < iterationCount; i++) { job.src[i] = (byte) i; }
			
			job.Run(iterationCount);
			
			// print first 16 elements
			for (int i = 0; i < 16; i++) { Console.WriteLine(job.dest[i]); }
			
			// dispose manually, you can also use 'using TestGpuJob job = new()'
			job.Dispose();
		}
		
		public class TestGpuJob : GpuJobBase {
			// use ClWrite for output arrays, ClRead for input arrays and ClReadWrite for both
			// first arg of this attribute - argument id of kernel
			// change second arg to false, if you want to manually manage memory (array.CopyToDevice(), array.CopyFromDevice() and array.Dispose()) (see OpenClTest_2.cs)
			[ClWrite(0)] public ClBuffer<byte> dest;
			[ClRead(1)] public ClBuffer<byte> src;

			protected override string progName => "TestProgram";
			protected override string kernelName => "TestKernel";

			protected override string programStr => @"
kernel void TestKernel(global uchar* dest, global uchar* src) {
	int index = get_global_id(0); // use get_global_id(0) to get iteration index
	dest[index] = src[index] * 2;
}
";

			protected override string[] programKernels => new[] { kernelName }; // all kernels that current program contain
		}
	}
}