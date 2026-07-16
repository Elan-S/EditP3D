using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Gibbed.IO
{
    public static partial class StreamHelpers
    {
        private static readonly Encoding FightDataStringEncoding = Encoding.ASCII;

        public static string ReadStringU32(this Stream stream, Endian endian)
        {
            return stream.ReadFightDataLengthPrefixedString((int)stream.ReadValueU32(endian));
        }

        public static void WriteStringU32(this Stream stream, string value, Endian endian)
        {
            stream.WriteFightDataLengthPrefixedString(value, endian, true, false);
        }

        public static void WriteStringU32NoTerminator(this Stream stream, string value, Endian endian)
        {
            stream.WriteFightDataLengthPrefixedString(value, endian, false, false);
        }

        public static string ReadStringAlignedU32(this Stream stream, Endian endian)
        {
            int length = (int)stream.ReadValueU32(endian);
            string value = stream.ReadFightDataLengthPrefixedString(length);
            stream.AlignFightDataRead(length, 4);
            return value;
        }

        public static void WriteStringAlignedU32(this Stream stream, string value, Endian endian)
        {
            stream.WriteFightDataLengthPrefixedString(value, endian, true, true);
        }

        private static string ReadFightDataLengthPrefixedString(this Stream stream, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            var data = new byte[length];
            if (stream.Read(data, 0, data.Length) != data.Length)
            {
                throw new EndOfStreamException();
            }

            if (data.Length > 0 && data[data.Length - 1] == 0)
            {
                length--;
            }

            return FightDataStringEncoding.GetString(data, 0, length);
        }

        private static void WriteFightDataLengthPrefixedString(this Stream stream, string value, Endian endian, bool terminator, bool aligned)
        {
            var data = FightDataStringEncoding.GetBytes(value ?? string.Empty);
            int length = data.Length + (terminator == true ? 1 : 0);
            stream.WriteValueU32((uint)length, endian);
            stream.Write(data, 0, data.Length);
            if (terminator == true)
            {
                stream.WriteByte(0);
            }

            if (aligned == true)
            {
                stream.AlignFightDataWrite(length, 4);
            }
        }

        private static void AlignFightDataRead(this Stream stream, int size, int align)
        {
            int skip = size % align;
            if (skip > 0)
            {
                stream.Seek(align - skip, SeekOrigin.Current);
            }
        }

        private static void AlignFightDataWrite(this Stream stream, int size, int align)
        {
            int skip = size % align;
            if (skip > 0)
            {
                var padding = new byte[align - skip];
                stream.Write(padding, 0, padding.Length);
            }
        }
    }

    public static class FightDataStringExtensions
    {
        public static string SeparateCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                return value;
            }

            return Regex.Replace(value, "([a-z0-9])([A-Z])", "$1 $2");
        }
    }
}
