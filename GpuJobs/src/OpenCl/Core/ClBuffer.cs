using System;
using GpuJobs.OpenCl.Api;
using GpuJobs.OpenCl.FrontEnd;
using GpuJobs.OpenCl.Utils;

namespace GpuJobs.OpenCl.Core {
	public class ClBuffer : ClMemObject {
		public IntPtr hostAllocation;
		public virtual int lengthInBytes { get; set; }
		public ClMemFlags flags;
		public bool useHostPtr;
		public OpenClDevice device;
		public ClAllocationPlace allocation;

		public ClBuffer(IntPtr handle) : base(handle) { }
		public unsafe ClBuffer(void* handle) : base(handle) { }

		public unsafe void CopyToDevice() {
			if ((allocation & ClAllocationPlace.Device) != 0) DisposeOnDevice();
			handle = (IntPtr) device.context.CreateBuffer(flags, lengthInBytes, (void*) hostAllocation, useHostPtr);
			allocation |= ClAllocationPlace.Device;
		}
		
		public void DisposeOnDevice() {
			if ((allocation & ClAllocationPlace.Device) != 0) ClMemApi.ClReleaseMemObject(handle);
			allocation &= ~ClAllocationPlace.Device;
		}
		
		public unsafe void CopyFromDevice() {
			device.cmdQueue.EnqueueReadBuffer(handle, lengthInBytes, (byte*) hostAllocation);
		}
		
		public void AllocateOnHost() {
			if ((allocation & ClAllocationPlace.Host) == 0) hostAllocation = MemManager.AllocateHeap(lengthInBytes);
			allocation |= ClAllocationPlace.Host;
		}
		
		public void DisposeOnHost() {
			if ((allocation & ClAllocationPlace.Host) != 0) MemManager.TryFreeHeap(hostAllocation);
			allocation &= ~ClAllocationPlace.Host;
		}

		protected override void ReleaseUnmanagedResources() {
			DisposeOnDevice();
			DisposeOnHost();
		}

		public unsafe byte this[int pos] {
			get => ((byte*) hostAllocation)[pos];
			set => ((byte*) hostAllocation)[pos] = value;
		}
	}

	public class ClBuffer<T> : ClBuffer where T : unmanaged {
		protected ClBuffer(IntPtr handle) : base(handle) { }
		protected unsafe ClBuffer(void* handle) : base(handle) { }

		public ClBuffer() : base(IntPtr.Zero) { }

		public int length;
		public override unsafe int lengthInBytes => length * sizeof(T);
		
		public unsafe T this[int pos] {
			get => ((T*) hostAllocation)[pos];
			set => ((T*) hostAllocation)[pos] = value;
		}
	}

	public static class ClBufferUtils {
		public static ClBuffer<T> AllocateOnHost<T>(this OpenClDevice device, int size, ClMemFlags flags, bool useHostPtr = false) where T : unmanaged {
			ClBuffer<T> buffer = new ClBuffer<T> {
				length = size, 
				flags = flags, 
				device = device
			};
			buffer.AllocateOnHost();
			return buffer;
		}
		
		public static ClBuffer<T> AllocateOnHost<T>(this OpenClDevice device, int size, bool useHostPtr = false) where T : unmanaged {
			ClBuffer<T> buffer = new ClBuffer<T> {
				length = size, 
				device = device
			};
			buffer.AllocateOnHost();
			return buffer;
		}
	}
	
	[Flags]
	public enum ClAllocationPlace : byte {
		NoAllocation = 0,
		Host = 1,
		Device = 2,
		All = Host | Device,
	}
}