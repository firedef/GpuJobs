using System;
using GpuJobs.OpenCl.Api;

namespace GpuJobs.OpenCl.Core {
	public class ClMemObject : ClHandle {
		public ClMemObject(IntPtr handle) : base(handle) { }
		public unsafe ClMemObject(void* handle) : base((IntPtr) handle) { }
		protected override void ReleaseUnmanagedResources() { ClMemApi.ClReleaseMemObject(handle); }
	}
}