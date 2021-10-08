using System;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.Core;
using GpuJobs.OpenCl.FrontEnd;

namespace GpuJobs.tests.simple {
	public static class OpenClTest_0 {
		public static void Run() {
			const string prog = @"
kernel void TestRun(global uchar* dest, global uchar* src) {
	int index = get_global_id(0);
	dest[index] = src[index] * 2;
}
";
			OpenClDevice device = OpenClCompute.WaitForAvailableDeviceAndPeak();
			device.TryAddKernelsFromString(prog, "TestProgram", new[] {"TestRun"});
			ClKernel kernel = device.GetKernel("TestProgram", "TestRun");
			ClBuffer<byte> arr_0 = device.AllocateOnHost<byte>(1024, ClMemFlags.CL_MEM_WRITE_ONLY);
			ClBuffer<byte> arr_1 = device.AllocateOnHost<byte>(1024, ClMemFlags.CL_MEM_READ_ONLY);
			for (int i = 0; i < 1024; i++) { arr_1[i] = (byte) i; }
			arr_0.CopyToDevice();
			arr_1.CopyToDevice();
			kernel.SetKernelArg(0, arr_0.handle);
			kernel.SetKernelArg(1, arr_1.handle);
			kernel.Run(device.cmdQueue, 1024);
			arr_0.CopyFromDevice();
			
			for (int i = 0; i < 16; i++) {
				Console.WriteLine(arr_0[i]);
			}
			
			arr_0.Dispose();
			arr_1.Dispose();
		}
	}
}