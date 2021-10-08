using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClCmdApi {
		[DllImport("OpenCL", EntryPoint = "clEnqueueReadBuffer")]
		public static extern unsafe ClResult ClEnqueueReadBuffer(
			IntPtr commandQueue,
			IntPtr buffer,
			uint blockingRead,
			UIntPtr offset,
			UIntPtr size,
			void* ptr,
			uint numEventsInWaitList,
			IntPtr* eventWaitList, // array
			IntPtr* event_
		);
		
		[DllImport("OpenCL", EntryPoint = "clEnqueueNDRangeKernel")]
		public static extern unsafe ClResult ClEnqueueNDRangeKernel(
			IntPtr commandQueue,
			IntPtr kernel,
			uint workDim,
			IntPtr* globalWorkOffset,
			IntPtr* globalWorkSize,
			IntPtr* localWorkSize,
			uint numEventsInWaitList,
			void* eventWaitList,
			IntPtr* event_
		);
		
		
	}
}