using System;

namespace GpuJobs.OpenCl.Api {
	[Flags]
	public enum ClMemFlags {
		CL_MEM_READ_WRITE = 1,
		CL_MEM_WRITE_ONLY = 2,
		CL_MEM_READ_ONLY = 4,
		CL_MEM_USE_HOST_PTR = 8,
		CL_MEM_ALLOC_HOST_PTR = 16,
		CL_MEM_COPY_HOST_PTR = 32,
		_RESERVED_ = 64,
		CL_MEM_HOST_WRITE_ONLY = 128,
		CL_MEM_HOST_READ_ONLY = 256,
		CL_MEM_HOST_NO_ACCESS = 512,
	}
}