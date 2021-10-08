using System;
using GpuJobs.tests.jobs;
using GpuJobs.tests.simple;
using GpuJobs.tests.string_distance;

namespace GpuJobs {
	internal class Program {
		public static void Main(string[] args) {
			OpenClTest_0.Run();
			OpenClTest_1.Run();
			OpenClTest_2.Run();

			Console.ReadKey();
		}
	}
}