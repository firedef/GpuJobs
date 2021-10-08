using System;
using GpuJobs.tests.jobs;
using GpuJobs.tests.simple;

namespace GpuJobs {
	internal class Program {
		public static void Main(string[] args) {
			OpenClTest_1.Run();

			Console.ReadKey();
		}
	}
}