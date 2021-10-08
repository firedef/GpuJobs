using System;
using GpuJobs.OpenCl.Api;

namespace GpuJobs.OpenCl.Core {
	public class ClCommandQueue : ClHandle {
		public ClCommandQueue(IntPtr handle) : base(handle) { }
		protected override void ReleaseUnmanagedResources() { ClCmdQueueApi.ClReleaseCommandQueue(handle); }
		
		public unsafe void EnqueueReadBuffer<T>(IntPtr objHandle, int size, T* trgt, bool wait = true) where T : unmanaged {
			int elementSize = sizeof(T);
			int totalSize = elementSize * size;

			IntPtr* waitEvent = stackalloc IntPtr[1];
			ClCmdApi.ClEnqueueReadBuffer(handle, objHandle, wait ? 1u : 0u, UIntPtr.Zero, (UIntPtr) totalSize, trgt, 0, null, waitEvent)
					.CheckResult($"EnqueueReadBuffer error");
		}
		
		public unsafe void EnqueueNDRangeKernel(ClKernel kernel, int workDim, int workUnitsPerKernel) {
			IntPtr* waitEventPtr = stackalloc IntPtr[1];
			IntPtr* globalWorkSize = stackalloc IntPtr[1];
			globalWorkSize[0] = (IntPtr) workUnitsPerKernel;
			
			ClCmdApi.ClEnqueueNDRangeKernel(handle, kernel.handle, (uint) workDim, null, globalWorkSize, null, 0, null, waitEventPtr)
					.CheckResult($"EnqueueReadBuffer error");
		}
	}
}