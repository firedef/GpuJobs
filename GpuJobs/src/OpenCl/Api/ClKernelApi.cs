using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClKernelApi {
		[DllImport("OpenCL", EntryPoint = "clCreateKernel")]
		public static extern IntPtr ClCreateKernel(
			IntPtr program,
			[MarshalAs(UnmanagedType.LPStr)] string kernelName,
			out ClResult errorCode
		);
		
		[DllImport("OpenCL", EntryPoint = "clReleaseKernel")]
		public static extern ClResult ClReleaseKernel(IntPtr kernel);
		
		[DllImport("OpenCL", EntryPoint = "clSetKernelArg")]
		public static extern unsafe ClResult ClSetKernelArg(
			IntPtr kernel,
			uint argumentIndex,
			UIntPtr argumentSize,
			void** argumentValue
		);
		
		[DllImport("OpenCL", EntryPoint = "clGetKernelInfo")]
		public static extern ClResult ClGetKernelInfo(
			IntPtr kernel,
			ClKernelInfo parameterName,
			UIntPtr parameterValueSize,
			byte[] parameterValue,
			out UIntPtr parameterValueSizeReturned
		);
	}
}