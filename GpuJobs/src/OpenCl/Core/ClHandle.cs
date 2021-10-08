using System;

namespace GpuJobs.OpenCl.Core {
	public abstract class ClHandle : IDisposable {
		public IntPtr handle;
		
		protected ClHandle(IntPtr handle) => this.handle = handle;

		protected abstract void ReleaseUnmanagedResources();

		public void Dispose() {
			try { ReleaseUnmanagedResources(); }
			catch (Exception e) { Console.WriteLine($"ClHandle dispose error: {e}"); }
		}
	}
}