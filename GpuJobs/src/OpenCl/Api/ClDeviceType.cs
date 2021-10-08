using System;

namespace GpuJobs.OpenCl.Api {
	// https://www.khronos.org/registry/OpenCL/sdk/2.2/docs/man/html/clGetDeviceIDs.html
	[Flags]
	public enum ClDeviceType : uint {
		Default = 1,
		Cpu = 2,
		Gpu = 4,
		Accelerator = 8,
		Custom = 16, // Dedicated accelerators that do not support programs written in an OpenCL kernel language,
		All = uint.MaxValue,
	}
}