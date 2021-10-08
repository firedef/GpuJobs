using System;
using GpuJobs.OpenCl.Api;

namespace GpuJobs.OpenCl.Core {
	public class ClProgram : ClHandle {
		public ClProgram(IntPtr handle) : base(handle) { }
		protected override void ReleaseUnmanagedResources() { ClProgramApi.ClReleaseProgram(handle); }
		
		public ClKernel CreateKernel(string kernelName)
		{
			IntPtr kernelPointer = ClKernelApi.ClCreateKernel(handle, kernelName, out ClResult result);
			result.CheckResult("CreateKernel error");
			return new(kernelPointer);
		}
	}
}