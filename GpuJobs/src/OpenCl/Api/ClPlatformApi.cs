using System;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Api {
	public static class ClPlatformApi {
		[DllImport("OpenCL", EntryPoint = "clGetPlatformIDs")]
		public static extern ClResult ClGetPlatformIDs(
			uint numEntries,
			IntPtr[] platforms,
			out uint numPlatforms
		);
	}
}