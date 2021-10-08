using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClMemApi {
		[DllImport("OpenCL", EntryPoint = "clCreateBuffer")]
		public static extern unsafe byte* ClCreateBuffer(
			IntPtr context,
			ClMemFlags flags,
			UIntPtr size,
			void* hostPointer,
			ClResult* errorCode
		);
		
		[DllImport("OpenCL", EntryPoint = "clReleaseMemObject")]
		public static extern ClResult ClReleaseMemObject(IntPtr handle);
	}
}