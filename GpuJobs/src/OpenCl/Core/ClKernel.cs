using System;
using GpuJobs.OpenCl.Api;

namespace GpuJobs.OpenCl.Core {
	public class ClKernel : ClHandle {
		public ClKernel(IntPtr handle) : base(handle) { }
		protected override void ReleaseUnmanagedResources() { ClKernelApi.ClReleaseKernel(handle); }
		
		public unsafe void SetKernelArg(int index, IntPtr h)
		{
			if (index < 0) throw new ClException($"kernel index #{index} is out of range");

			ClKernelApi.ClSetKernelArg(handle, (uint) index, (UIntPtr) (Environment.Is64BitProcess ? 8u : 4u), (void**) &h)
					   .CheckResult($"error when set #{index} kernel arg");
		}
		
		public unsafe void SetKernelArg(int index, void* h)
		{
			if (index < 0) throw new ClException($"kernel index #{index} is out of range");

			ClKernelApi.ClSetKernelArg(handle, (uint) index, (UIntPtr) (Environment.Is64BitProcess ? 8u : 4u), &h)
					   .CheckResult($"error when set #{index} kernel arg");
		}
		
		public unsafe void PrepareLocalKernelArg(int index, int sizeInBytes)
		{
			if (index < 0) throw new ClException($"kernel index #{index} is out of range");

			ClKernelApi.ClSetKernelArg(handle, (uint) index, (UIntPtr) sizeInBytes, null)
					   .CheckResult($"kernel index #{index} is out of range");
		}

		public void Run(ClCommandQueue q, int iterations) {
			q.EnqueueNDRangeKernel(this, 1, iterations);
		}
	}
}