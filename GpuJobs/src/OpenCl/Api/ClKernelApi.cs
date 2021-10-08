using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClKernelApi {
		[DllImport("OpenCL", EntryPoint = "clCreateKernel")]
		public static extern IntPtr ClCreateKernel(
			IntPtr program,
			string kernelName,
			out int errorCode
		);
		
		
	}
}