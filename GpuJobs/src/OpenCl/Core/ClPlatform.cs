using System;
using GpuJobs.OpenCl.Api;

namespace GpuJobs.OpenCl.Core {
	public class ClPlatform : ClHandle {
		public ClPlatform(IntPtr handle) : base(handle) { }
		
		// no platform release, because it`s not a gpu object
		// https://stackoverflow.com/questions/17711407/opencl-releasing-platform-object
		protected override void ReleaseUnmanagedResources() { }
		
		public ClDevice[] GetDevices(ClDeviceType type = ClDeviceType.All) {
			ClDeviceApi.ClGetDeviceIDs(handle, type, 0, null, out uint availableDeviceCount)
					   .CheckResult("ClGetDeviceIDs error #0");

			IntPtr[] devicePointers = new IntPtr[availableDeviceCount];
			ClDeviceApi.ClGetDeviceIDs(handle, type, availableDeviceCount, devicePointers, out availableDeviceCount)
					   .CheckResult("ClGetDeviceIDs error #1");

			ClDevice[] devices = new ClDevice[availableDeviceCount];
			for (int i = 0; i < availableDeviceCount; i++) devices[i] = new(devicePointers[i]);
			return devices;
		}

		public static ClPlatform[] GetPlatforms() {
			ClPlatformApi.ClGetPlatformIDs(0, null, out uint availablePlatformCount)
						 .CheckResult("GetPlatforms error");
            
			IntPtr[] platformPointers = new IntPtr[availablePlatformCount];
			ClPlatformApi.ClGetPlatformIDs(availablePlatformCount, platformPointers, out _)
						 .CheckResult("GetPlatforms error");

			ClPlatform[] platforms = new ClPlatform[availablePlatformCount];
			for (int i = 0; i < availablePlatformCount; i++) {
				platforms[i] = new(platformPointers[i]);
			}

			return platforms;
		}
	}
}