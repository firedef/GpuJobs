using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClKernelApi {
		[DllImport("OpenCL", EntryPoint = "clCreateKernel")]
		public static extern IntPtr ClCreateKernel(
			IntPtr program,
			[MarshalAs(UnmanagedType.LPStr)] string kernelName,
			[Out] [MarshalAs(UnmanagedType.I4)] out int errorCode
		);
		
		
	}
}