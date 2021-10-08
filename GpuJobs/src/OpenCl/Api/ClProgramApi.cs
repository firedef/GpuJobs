using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClProgramApi {
		[DllImport("OpenCL", EntryPoint = "clCreateProgramWithSource")]
		public static extern IntPtr ClCreateProgramWithSource(
			IntPtr context,
			uint count,
			[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] strings,
			UIntPtr[] lengths,
			out ClResult errorCode
		);
		
		[DllImport("OpenCL", EntryPoint = "clReleaseProgram")]
		public static extern ClResult ClReleaseProgram(IntPtr program);
		
		[DllImport("OpenCL", EntryPoint = "clBuildProgram")]
		public static extern ClResult ClBuildProgram(
			IntPtr program,
			uint numberOfDevices,
			IntPtr[] deviceList,
			[MarshalAs(UnmanagedType.LPStr)] string options,
			IntPtr notificationCallback,
			IntPtr userData
		);
	}
}