using System;
using System.Linq;
using System.Text;
using GpuJobs.Jobs;
using GpuJobs.OpenCl.Core;

namespace GpuJobs.tests.string_distance {
	// compute levenshtein string distance
	// algorithm uses bytes (uchars) for lengths and symbols, so it's
	// supports UTF8 strings with length < (255 - targetString.length) and targetString with length < 255
	// in my project, i use ~100k strings and it's computing in a few milliseconds, but for small amount of data, computing on cpu much more efficient
	//! SPECIFY PATH TO .CL FILE IN ComputeDistance.programStr
	public static class OpenClTest_2 {
		public static void Run() {
			string targetString = "driver";
			string[] strings = { "driver", "drivers", "driving", "dry", "distance", "soup", "veryLongStringText", "eeeee" };
			
			int strCount = strings.Length;
			int maxBlockSize = strings.Select(str => str.Length).Max() + 1; // calc longest string length

			using ComputeDistance job = new(); // auto-dispose
			ClBuffer<byte> stringsArr = job.device.AllocateOnHost<byte>(maxBlockSize * strCount);
			ClBuffer<byte> targetStringArr = job.device.AllocateOnHost<byte>(targetString.Length+1);

			for (int i = 0; i < strCount; i++) {
				int pos = i * maxBlockSize;
				byte[] strByteArr = Encoding.UTF8.GetBytes(strings[i]); // convert string to utf8 bytes
				stringsArr[pos] = (byte) strByteArr.Length; // write length of the string to start
				for (int j = 0; j < strByteArr.Length; j++) stringsArr[pos + j + 1] = strByteArr[j]; // copy string to array
			}

			{
				byte[] strByteArr = Encoding.UTF8.GetBytes(targetString);
				targetStringArr[0] = (byte) strByteArr.Length;
				for (int j = 0; j < strByteArr.Length; j++) targetStringArr[j + 1] = strByteArr[j];
			}

			job.strings = stringsArr;
			job.targetString = targetStringArr;
			job.distances = job.device.AllocateOnHost<byte>(strCount);
			job.args = job.device.AllocateOnHost<int>(1);
			job.args[0] = maxBlockSize; // pass max string length to kernel

			job.Run(strCount);

			for (int i = 0; i < strCount; i++) {
				Console.WriteLine($"{targetString} {strings[i]}: {job.distances[i]}");
			}
		}

		public class ComputeDistance : GpuJobBase {
			[ClRead(0)] public ClBuffer<byte> strings;
			[ClRead(1)] public ClBuffer<byte> targetString;
			[ClWrite(2)] public ClBuffer<byte> distances;
			[ClRead(3)] public ClBuffer<int> args;

			protected override string kernelName => "RunKernel";
			
			protected override string progName => "ComputeStringDistance";
			// path to .cl file
			protected override string programStr => @"D:\projects\cs\GpuJobs\GpuJobs\tests\string distance\ComputeStringDistanceCl.cl";
			protected override string[] programKernels => new[] { kernelName }; // all kernels, that cl program contain
			protected override bool usePathInsteadOfSrc => true; // you can use path, instead of source by setting it to true
		}
	}
}