using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GpuJobs.OpenCl.Utils {
	public static unsafe class MemManager {
		public static IntPtr processHeap = _GetProcessHeap();
		
		[DllImport("kernel32.dll", EntryPoint = "HeapAlloc")]
		private static extern IntPtr _HeapAlloc(IntPtr hHeap, int dwFlags, IntPtr dwBytes);
		
		[DllImport("kernel32.dll", EntryPoint = "HeapFree")]
		private static extern int _HeapFree(IntPtr hHeap, int dwFlags, IntPtr lpMem);
		
		[DllImport("kernel32.dll", EntryPoint = "VirtualAlloc")]
		private static extern IntPtr _VirtualAlloc(IntPtr lpAddress, IntPtr dwSize, int flAllocationType, int flProtect);
		
		[DllImport("kernel32.dll", EntryPoint = "VirtualFree")]
		private static extern int _VirtualFree(IntPtr lpAddress, IntPtr dwSize, int dwFreeType);
		
		[DllImport("kernel32.dll", EntryPoint = "MapViewOfFile")]
		private static extern IntPtr _MapViewOfFile(IntPtr hFileMappingObject, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);
		
		[DllImport("kernel32.dll", EntryPoint = "GetProcessHeap")]
		private static extern IntPtr _GetProcessHeap();

		public static Dictionary<IntPtr, int> allocations_heap = new();

		public static IntPtr AllocateHeap(int size) {
			IntPtr ptr = _HeapAlloc(processHeap, 0, (IntPtr) size);
			allocations_heap.Add(ptr, size);
			return ptr;
		}
		public static T* AllocateHeap<T>(int size) where T : unmanaged => (T*) AllocateHeap(size * sizeof(T));

		public static void FreeHeap(IntPtr ptr) {
			_HeapFree(processHeap, 0, ptr);
			allocations_heap.Remove(ptr);
		}
		
		public static void FreeHeap<T>(T* ptr) where T : unmanaged => FreeHeap((IntPtr) ptr);

		public static bool TryFreeHeap(IntPtr ptr) {
			if (allocations_heap.Remove(ptr)) {
				_HeapFree(processHeap, 0, ptr);
				return true;
			}

			return false;
		}
		
		public static bool TryFreeHeap<T>(T* ptr) where T : unmanaged => TryFreeHeap((IntPtr) ptr);

	}
}