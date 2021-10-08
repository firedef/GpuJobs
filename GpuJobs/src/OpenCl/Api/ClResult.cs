namespace GpuJobs.OpenCl.Api {
	// https://streamhpc.com/blog/2013-04-28/opencl-error-codes/
	public enum ClResult : int {
		// Run-time and JIT Compiler Errors (driver-dependent)
		CL_SUCCESS = 0,
		CL_DEVICE_NOT_FOUND = -1,
		CL_DEVICE_NOT_AVAILABLE = -2,
		CL_COMPILER_NOT_AVAILABLE = -3,
		CL_MEM_OBJECT_ALLOCATION_FAILURE = -4,
		CL_OUT_OF_RESOURCES = -5,
		CL_OUT_OF_HOST_MEMORY = -6,
		CL_PROFILING_INFO_NOT_AVAILABLE = -7,
		CL_MEM_COPY_OVERLAP = -8,
		CL_IMAGE_FORMAT_MISMATCH = -9,
		CL_IMAGE_FORMAT_NOT_SUPPORTED = -10,
		CL_BUILD_PROGRAM_FAILURE = -11,
		CL_MAP_FAILURE = -12,
		CL_MISALIGNED_SUB_BUFFER_OFFSET = -13,
		CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST = -14,
		CL_COMPILE_PROGRAM_FAILURE = -15,
		CL_LINKER_NOT_AVAILABLE = -16,
		CL_LINK_PROGRAM_FAILURE = -17,
		CL_DEVICE_PARTITION_FAILED = -18,
		CL_KERNEL_ARG_INFO_NOT_AVAILABLE = -19,
		
		// Compile-time Errors (driver-independent)
		CL_INVALID_VALUE = -30,
		CL_INVALID_DEVICE_TYPE = -31,
		CL_INVALID_PLATFORM = -32,
		CL_INVALID_DEVICE = -33,
		CL_INVALID_CONTEXT = -34,
		CL_INVALID_QUEUE_PROPERTIES = -35,
		CL_INVALID_COMMAND_QUEUE = -36,
		CL_INVALID_HOST_PTR = -37,
		CL_INVALID_MEM_OBJECT = -38,
		CL_INVALID_IMAGE_FORMAT_DESCRIPTOR = -39,
		CL_INVALID_IMAGE_SIZE = -40,
		CL_INVALID_SAMPLER = -41,
		CL_INVALID_BINARY = -42,
		CL_INVALID_BUILD_OPTIONS = -43,
		CL_INVALID_PROGRAM = -44,
		CL_INVALID_PROGRAM_EXECUTABLE = -45,
		CL_INVALID_KERNEL_NAME = -46,
		CL_INVALID_KERNEL_DEFINITION = -47,
		CL_INVALID_KERNEL = -48,
		CL_INVALID_ARG_INDEX = -49,
		CL_INVALID_ARG_VALUE = -50,
		CL_INVALID_ARG_SIZE = -51,
		CL_INVALID_KERNEL_ARGS = -52,
		CL_INVALID_WORK_DIMENSION = -53,
		CL_INVALID_WORK_GROUP_SIZE = -54,
		CL_INVALID_WORK_ITEM_SIZE = -55,
		CL_INVALID_GLOBAL_OFFSET = -56,
		CL_INVALID_EVENT_WAIT_LIST = -57,
		CL_INVALID_EVENT = -58,
		CL_INVALID_OPERATION = -59,
		CL_INVALID_GL_OBJECT = -60,
		CL_INVALID_BUFFER_SIZE = -61,
		CL_INVALID_MIP_LEVEL = -62,
		CL_INVALID_GLOBAL_WORK_SIZE = -63,
		CL_INVALID_PROPERTY = -64,
		CL_INVALID_IMAGE_DESCRIPTOR = -65,
		CL_INVALID_COMPILER_OPTIONS = -66,
		CL_INVALID_LINKER_OPTIONS = -67,
		CL_INVALID_DEVICE_PARTITION_COUNT = -68,
		CL_INVALID_PIPE_SIZE = -69,
		CL_INVALID_DEVICE_QUEUE = -70,
		
		// Errors thrown by extensions
		CL_INVALID_GL_SHAREGROUP_REFERENCE_KHR = -1000,
		CL_PLATFORM_NOT_FOUND_KHR = -1001,
		CL_INVALID_D3D10_DEVICE_KHR = -1002,
		CL_INVALID_D3D10_RESOURCE_KHR = -1003,
		CL_D3D10_RESOURCE_ALREADY_ACQUIRED_KHR = -1004,
		CL_D3D10_RESOURCE_NOT_ACQUIRED_KHR = -1005,
		CL_INVALID_D3D11_DEVICE_KHR = -1006,
		CL_INVALID_D3D11_RESOURCE_KHR = -1007,
		CL_D3D11_RESOURCE_ALREADY_ACQUIRED_KHR = -1008,
		CL_D3D11_RESOURCE_NOT_ACQUIRED_KHR = -1009,
		CL_INVALID_D3D9_DEVICE_NV = -1010,
		CL_INVALID_D3D9_RESOURCE_NV = -1011,
		CL_D3D9_RESOURCE_ALREADY_ACQUIRED_NV = -1012,
		CL_D3D9_RESOURCE_NOT_ACQUIRED_NV  = -1013,
		CL_EGL_RESOURCE_NOT_ACQUIRED_KHR = -1092,
		CL_INVALID_EGL_OBJECT_KHR = -1093,
		CL_INVALID_ACCELERATOR_INTEL = -1094,
		CL_INVALID_ACCELERATOR_TYPE_INTEL = -1095,
		CL_INVALID_ACCELERATOR_DESCRIPTOR_INTEL = -1096,
		CL_ACCELERATOR_TYPE_NOT_SUPPORTED_INTEL = -1097,
		CL_INVALID_VA_API_MEDIA_ADAPTER_INTEL = -1098,
		CL_INVALID_VA_API_MEDIA_SURFACE_INTEL = -1099,
		CL_VA_API_MEDIA_SURFACE_ALREADY_ACQUIRED_INTEL = -1100,
		CL_VA_API_MEDIA_SURFACE_NOT_ACQUIRED_INTEL = -1101,
		
		// Errors thrown by Vendors
		Nvidia_IllegalReadOrWriteToBuffer = -9999,
	}
}