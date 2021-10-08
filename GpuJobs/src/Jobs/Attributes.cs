using System;

namespace GpuJobs.Jobs {
	[AttributeUsage(AttributeTargets.Field)]
	public class ClReadAttribute : Attribute {
		public int argId;
		public bool autoMemManagement;

		public ClReadAttribute(int argId, bool autoMemManagement = true) {
			this.argId = argId;
			this.autoMemManagement = autoMemManagement;
		}
	}
	
	[AttributeUsage(AttributeTargets.Field)]
	public class ClWriteAttribute : Attribute {
		public int argId;
		public bool autoMemManagement;

		public ClWriteAttribute(int argId, bool autoMemManagement = true) {
			this.argId = argId;
			this.autoMemManagement = autoMemManagement;
		}
	}
	
	[AttributeUsage(AttributeTargets.Field)]
	public class ClReadWriteAttribute : Attribute {
		public int argId;
		public bool autoMemManagement;

		public ClReadWriteAttribute(int argId, bool autoMemManagement = true) {
			this.argId = argId;
			this.autoMemManagement = autoMemManagement;
		}
	}
}