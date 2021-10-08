using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClContextApi {
		[DllImport("OpenCL", EntryPoint = "clCreateContext")]
		public static extern IntPtr ClCreateContext(
			IntPtr properties,
			uint num_devices,
			IntPtr[] devices,
			IntPtr pfn_notify,
			IntPtr user_data,
			out ClResult errorCode);
		
		[DllImport("OpenCL", EntryPoint = "clReleaseContext")]
		public static extern ClResult ClReleaseContext(IntPtr context);
	}
}