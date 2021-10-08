using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClDeviceApi {
		[DllImport("OpenCL", EntryPoint = "clGetDeviceIDs")]
		public static extern ClResult ClGetDeviceIDs(
			IntPtr platform,
			ClDeviceType deviceType,
			uint numberOfEntries,
			IntPtr[] devices,
			out uint numberOfDevicesReturned
		);
		
		[DllImport("OpenCL", EntryPoint = "clGetDeviceInfo")]
		public static extern ClResult ClGetDeviceInfo(
			IntPtr device,
			ClDeviceInfo param_name,
			UIntPtr param_value_size,
			byte[] param_value,
			out UIntPtr param_value_size_ret
		);
		
		[DllImport("OpenCL", EntryPoint = "clReleaseDevice")]
		public static extern ClResult ClReleaseDevice(IntPtr device);
	}
}