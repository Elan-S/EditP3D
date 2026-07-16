using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Gibbed.IO;

namespace Nixson.Common
{
	public static class Utils
	{
		public static List<string> StringToList(Stream input)
		{
			List<string> list = new List<string>();
			int num = input.ReadValueS32();
			string text = "";
			for (int i = 0; i < num; i++)
			{
				char c = (char)input.ReadByte();
				if (c == ';')
				{
					list.Add(text);
					text = "";
				}
				else
				{
					text += c.ToString();
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
			return list;
		}
		public static List<string> StringToList(string str)
		{
			return str.Split(new char[]
			{
				';'
			}).ToList<string>();
		}
		public static string ListToString(List<string> list)
		{
			string text = string.Empty;
			for (int i = 0; i < list.Count; i++)
			{
				text += list[i];
				if (i != list.Count - 1)
				{
					text += ";";
				}
			}
			return text;
		}
		public static T StringToEnum<T>(string value) where T : struct
		{
			if (string.IsNullOrEmpty(value))
			{
				return default(T);
			}
			T result;
			if (!Enum.TryParse<T>(value, out result))
			{
				throw new FormatException("Invalid enum value");
			}
			return result;
		}
		public static T IntToEnum<T>(int index) where T : struct
		{
			return (T)((object)Enum.ToObject(typeof(T), index));
		}
		public static Stream CompressRZ(Stream input)
		{
			MemoryStream memoryStream = new MemoryStream();
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream2, CompressionMode.Compress))
				{
					input.CopyTo(deflateStream);
					deflateStream.Close();
					byte[] data = memoryStream2.ToArray();
					memoryStream.WriteValueU32(23122U);
					memoryStream.WriteValueU32(0U);
					memoryStream.WriteValueS32((int)input.Length);
					memoryStream.WriteValueU32(0U);
					memoryStream.WriteValueU16(40056);
					memoryStream.WriteBytes(data);
					memoryStream.WriteValueU32(Utils.Adler32(input), Endian.Big);
				}
			}
			return memoryStream;
		}
		public static void DecompressRZ(Stream input, Stream output)
		{
			using (DeflateStream deflateStream = new DeflateStream(input, CompressionMode.Decompress))
			{
				input.Position = 18L;
				deflateStream.CopyTo(output);
			}
		}
		public static bool IsRZFile(string path)
		{
			return path.ToLower().EndsWith(".p3d.rz");
		}
		private static uint Adler32(Stream input)
		{
			uint num = 1U;
			uint num2 = 0U;
			MemoryStream memoryStream = new MemoryStream();
			input.Position = 0L;
			input.CopyTo(memoryStream);
			byte[] array = memoryStream.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				num = (num + (uint)array[i]) % 65521U;
				num2 = (num2 + num) % 65521U;
			}
			return num2 << 16 | num;
		}
		public static uint RCFStringHash(string str)
		{
			uint num = 0U;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (i != 0 || c != '\\')
				{
					uint num2 = (uint)((c < 'a') ? (c + 'a' - 'A') : c);
					num = (num << 5) - num + num2;
				}
			}
			return num;
		}
		public static uint GetUnixEpoch(this DateTime dateTime)
		{
			return (uint)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}
	}
}
