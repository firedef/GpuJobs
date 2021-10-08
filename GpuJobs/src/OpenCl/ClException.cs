using System;
using GpuJobs.OpenCl.Api;

namespace GpuJobs.OpenCl {
	public class ClException : Exception {
		public ClException(string msg) : base(msg) { }
		public ClException(string msg, ClResult result) : base($"{msg}: {result}") { }
		public ClException(string msg, ClResult result, string err) : base($"{msg}: {result}\n{err}") { }
	}
}