using System;
using GpuJobs.Jobs;
using GpuJobs.OpenCl.Core;

namespace GpuJobs.tests.jobs {
	public static class OpenClTest_1 {
		public static void Run() {
			TestGpuJob job = new();
			
			job.dest = job.device.AllocateOnHost<byte>(1024);
			job.src = job.device.AllocateOnHost<byte>(1024);
			
			for (int i = 0; i < 1024; i++) { job.src[i] = (byte) i; }
			
			job.Run(1024);
			
			for (int i = 0; i < 16; i++) { Console.WriteLine(job.dest[i]); }
			
			job.Dispose();
		}
		
		public class TestGpuJob : GpuJobBase {
			[ClWrite(0)] public ClBuffer<byte> dest;
			[ClRead(1)] public ClBuffer<byte> src;

			protected override string progName => "TestProgram";
			protected override string kernelName => "TestKernel";

			protected override string programStr => @"
kernel void TestKernel(global uchar* dest, global uchar* src) {
	int index = get_global_id(0);
	dest[index] = src[index] * 2;
}
";

			protected override string[] programKernels => new[] { kernelName };
		}
	}
}