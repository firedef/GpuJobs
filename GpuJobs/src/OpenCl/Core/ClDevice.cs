using System;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.Utils;

namespace GpuJobs.OpenCl.Core {
	public class ClDevice : ClHandle {
		// https://www.khronos.org/registry/OpenCL/sdk/2.2/docs/man/html/clGetDeviceInfo.html
		public string name => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_NAME).AsString();
		public ClDeviceType type => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_TYPE).AsEnum<ClDeviceType>();
		public uint computeUnits => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_MAX_COMPUTE_UNITS).AsU32();
		public uint maxClockFrq => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_MAX_CLOCK_FREQUENCY).AsU32();
		public uint addressBits => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_ADDRESS_BITS).AsU32();
		public ulong maxMemObjSize => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_MAX_MEM_ALLOC_SIZE).AsU64();
		public IntPtr maxImage2DWidth => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_IMAGE2D_MAX_WIDTH).AsIntPtr();
		public IntPtr maxImage2DHeight => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_IMAGE2D_MAX_HEIGHT).AsIntPtr();
		public IntPtr maxImage3DWidth => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_IMAGE3D_MAX_WIDTH).AsIntPtr();
		public IntPtr maxImage3DHeight => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_IMAGE3D_MAX_HEIGHT).AsIntPtr();
		public IntPtr maxImage3DDepth => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_IMAGE3D_MAX_DEPTH).AsIntPtr();
		public IntPtr maxImgBufferSize => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_IMAGE_MAX_BUFFER_SIZE).AsIntPtr();
		public IntPtr maxKernelArgSize => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_MAX_PARAMETER_SIZE).AsIntPtr();
		public string driverVersion => GetDeviceInformation(ClDeviceInfo.CL_DRIVER_VERSION).AsString();
		public string deviceExtensions => GetDeviceInformation(ClDeviceInfo.CL_DEVICE_EXTENSIONS).AsString();
		
		public ClDevice(IntPtr handle) : base(handle) { }
		protected override void ReleaseUnmanagedResources() { ClDeviceApi.ClReleaseDevice(handle); }

		public unsafe ClCommandQueue CreateQueue(ClContext ctx) {
			ClResult* result = stackalloc ClResult[1];
			IntPtr cmdQueuePtr = ClCmdQueueApi.ClCreateCommandQueueWithProperties(ctx.handle, handle, IntPtr.Zero, result);
			result->CheckResult($"CreateQueue error");
			return new(cmdQueuePtr);
		}
		
		private byte[] GetDeviceInformation(ClDeviceInfo propName)
		{
			ClDeviceApi.ClGetDeviceInfo(handle, propName, UIntPtr.Zero, null, out UIntPtr valueSize)
					   .CheckResult($"GetDeviceInformation error");
            
			byte[] output = new byte[(int) valueSize];
			ClDeviceApi.ClGetDeviceInfo(handle, propName, new((uint)output.Length), output, out _)
					   .CheckResult($"GetDeviceInformation error");
			return output;
		}

		public override string ToString() => $"{type} {name} {maxClockFrq}hz OpenCl{driverVersion}";
	}
}