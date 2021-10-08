namespace GpuJobs.OpenCl.Api {
	// https://docs.rs/ocl/0.9.0/i686-apple-darwin/src/ocl/.cargo/registry/src/github.com-1ecc6299db9ec823/ocl-0.9.0/src/cl_h.rs.html#485
	public enum ClKernelInfo : uint {
		CL_KERNEL_FUNCTION_NAME = 0x1190,
		CL_KERNEL_NUM_ARGS = 0x1191,
		CL_KERNEL_REFERENCE_COUNT = 0x1192,
		CL_KERNEL_CONTEXT = 0x1193,
		CL_KERNEL_PROGRAM = 0x1194,
		CL_KERNEL_ATTRIBUTES = 0x1195,
	}
}