using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.Core;

namespace GpuJobs.OpenCl.FrontEnd {
	public static class OpenClCompute {
		public static OpenClDevice[] devices;
		public static ConcurrentQueue<int> availableDevices = new();

		static OpenClCompute() {
			if (!Environment.Is64BitProcess) throw new NotSupportedException($"32-bit is not supported by GpuJobs lib yet!");
			List<OpenClDevice> deviceList = new();
			foreach (ClPlatform platform in ClPlatform.GetPlatforms()) {
				ClDevice[] clDevices = platform.GetDevices();
				foreach (ClDevice device_ in clDevices) {
					ClContext ctx = ClContext.CreateContext(device_);
					ClCommandQueue q = device_.CreateQueue(ctx);
					availableDevices.Enqueue(deviceList.Count);
					OpenClDevice newDevice = new(deviceList.Count, device_, ctx, q);
					deviceList.Add(newDevice);
					Console.WriteLine($"found OpenCl device: {newDevice}");
				}
			}

			devices = deviceList.ToArray();
		}

		public static OpenClDevice WaitForAvailableDeviceAndPeak() {
			while (true) {
				if (!availableDevices.IsEmpty && availableDevices.TryPeek(out int pos)) return devices[pos];
				Thread.SpinWait(10);
			}
		}
		
		public static OpenClDevice WaitForAvailableDeviceAndPop() {
			while (true) {
				if (!availableDevices.IsEmpty && availableDevices.TryDequeue(out int pos)) return devices[pos];
				Thread.SpinWait(10);
			}
		}
	}
}