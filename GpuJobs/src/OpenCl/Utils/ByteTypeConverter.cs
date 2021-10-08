using System;
using System.Text;

namespace GpuJobs.OpenCl.Utils {
	public static class ByteTypeConverter {
		public static string AsString(this byte[] data) => Encoding.ASCII.GetString(data).Replace("\0","");

		public static bool AsBool(this byte[] data) => data.AsI32() != 0;
		
		public static sbyte AsI8(this byte[] data) => (sbyte) data[0];
		public static byte AsU8(this byte[] data) => data[0];
		public static short AsI16(this byte[] data) => BitConverter.ToInt16(data, 0);
		public static ushort AsU16(this byte[] data) => BitConverter.ToUInt16(data, 0);
		public static int AsI32(this byte[] data) => BitConverter.ToInt32(data, 0);
		public static uint AsU32(this byte[] data) => BitConverter.ToUInt32(data, 0);
		public static long AsI64(this byte[] data) => BitConverter.ToInt64(data, 0);
		public static ulong AsU64(this byte[] data) => BitConverter.ToUInt64(data, 0);

		public static IntPtr AsIntPtr(this byte[] data) => Environment.Is64BitProcess ? (IntPtr) AsI64(data) : (IntPtr) AsI32(data);
		public static UIntPtr AsUIntPtr(this byte[] data) => Environment.Is64BitProcess ? (UIntPtr) AsU64(data) : (UIntPtr) AsU32(data);

		public static unsafe T AsEnum<T>(this byte[] data) where T : unmanaged, Enum {
			Type baseType = Enum.GetUnderlyingType(typeof(T));

			long? v = null;
			if (baseType == typeof(byte)) v = data.AsU8();
			else if (baseType == typeof(sbyte)) v = data.AsI8();
			else if (baseType == typeof(ushort)) v = data.AsU16();
			else if (baseType == typeof(short)) v = data.AsI16();
			else if (baseType == typeof(uint)) v = data.AsU32();
			else if (baseType == typeof(int)) v = data.AsI32();
			else if (baseType == typeof(ulong)) v = (long) data.AsU64();
			else if (baseType == typeof(long)) v = data.AsI64();
			else if (baseType == typeof(UIntPtr)) v = (long) data.AsUIntPtr();
			else if (baseType == typeof(IntPtr)) v = (long) data.AsIntPtr();

			if (v != null) return *(T*) &v;
			
			throw new IndexOutOfRangeException($"invalid enum type: {baseType.FullName}");
		}
		
		public static unsafe T AsEnum<T>(this byte[] data, int size, bool signed) where T : unmanaged, Enum {
			long v = (size, signed) switch {
				(08, false) => data.AsU8(), 
				(08, true) => data.AsI8(), 
				(16, false) => data.AsU16(), 
				(16, true) => data.AsI16(), 
				(32, false) => data.AsU32(), 
				(32, true) => data.AsI32(), 
				(64, false) => (long) data.AsU64(), 
				(64, true) => data.AsI64(), 
				_ => throw new IndexOutOfRangeException($"{size} must be power of 8/16/32/64")
			};
			
			return *(T*) &v;
		}
	}
}