using System;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.Core;
using GpuJobs.OpenCl.FrontEnd;

namespace GpuJobs.tests.simple {
	public static class OpenClTest_0 {
		public static void Run() {
			// OpenCl program that multiplies src array by 2
			const string prog = @"
kernel void TestRun(global uchar* dest, global uchar* src) {
	int index = get_global_id(0); // get iteration index
	dest[index] = src[index] * 2;
}
";
			// get free device and make it inaccessible for other threads
			// you can use WaitForAvailableDeviceAndPeak() for remove block, but it can sometimes throw errors
			OpenClDevice device = OpenClCompute.WaitForAvailableDeviceAndPop();
			
			// build program
			device.TryAddKernelsFromString(prog, "TestProgram0", new[] {"TestRun"});
			ClKernel kernel = device.GetKernel("TestProgram0", "TestRun");
			
			// allocate all arrays before Run
			ClBuffer<byte> arr_0 = device.AllocateOnHost<byte>(1024, ClMemFlags.CL_MEM_WRITE_ONLY);
			ClBuffer<byte> arr_1 = device.AllocateOnHost<byte>(1024, ClMemFlags.CL_MEM_READ_ONLY);
			
			// you can also fill arrays
			for (int i = 0; i < 1024; i++) { arr_1[i] = (byte) i; }
			
			// send arrays to device
			// you also must send write arrays
			arr_0.CopyToDevice();
			arr_1.CopyToDevice();
			
			// set arrays to arguments
			kernel.SetKernelArg(0, arr_0.handle);
			kernel.SetKernelArg(1, arr_1.handle);
			
			// run kernel
			kernel.Run(device.cmdQueue, 1024);
			
			// copy write arrays back to cpu
			arr_0.CopyFromDevice();
			
			// return device back to pool (do not confuse with Dispose()) 
			device.Free();
			
			// print first 16 elements of write array
			for (int i = 0; i < 16; i++) { Console.WriteLine(arr_0[i]); }
			
			// dispose arrays
			arr_0.Dispose();
			
			// you can also dispose only on device/host
			arr_1.DisposeOnDevice();
			arr_1.DisposeOnHost();
		}
	}
}