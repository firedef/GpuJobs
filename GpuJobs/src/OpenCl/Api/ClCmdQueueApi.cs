using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClCmdQueueApi {
		[DllImport("OpenCL", EntryPoint = "clCreateCommandQueueWithProperties")]
		public static extern unsafe IntPtr ClCreateCommandQueueWithProperties(
			IntPtr context,
			IntPtr device,
			IntPtr properties,
			ClResult* errorCode
		);
		
		[DllImport("OpenCL", EntryPoint = "clReleaseCommandQueue")]
		public static extern ClResult ClReleaseCommandQueue(IntPtr commandQueue);
		
		[DllImport("OpenCL", EntryPoint = "clFlush")]
		public static extern ClResult ClFlush(IntPtr commandQueue);
		
		[DllImport("OpenCL", EntryPoint = "clFinish")]
		public static extern ClResult ClFinish(IntPtr commandQueue);
	}
}