using System;
using System.IO;
using System.Text;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight
{
	public class FightFile
	{
		public uint Flags { get; set; }
		public uint Padding { get; set; }
		public FightChunk Chunk { get; set; }
		public void Deserialize(PrototypeGame game, Stream input, Endian endianess)
		{
			ulong position = (ulong)input.Position;
			uint num = input.ReadValueU32(endianess);
			if (num != 4282659664U && num.Swap() != 4282659664U)
			{
				throw new FormatException("Not a Pure3D file");
			}
			if (input.ReadValueU32(endianess) != 12U)
			{
				throw new FormatException("Invalid header size");
			}
			uint num2 = input.ReadValueU32(endianess);
			if (position + (ulong)num2 > (ulong)input.Length)
			{
				throw new FormatException();
			}
			if (input.ReadValueU32() != 536872706U)
			{
				throw new FormatException("Not a fig data p3d");
			}
			input.ReadBytes(8);
			this.DeserializeFig(game, input, endianess);
		}
		public void DeserializeFig(PrototypeGame game, Stream input, Endian endianess)
		{
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			if (input.ReadString(4, Encoding.ASCII) != "fig0")
			{
				throw new FormatException("Not a fight node");
			}
			this.Flags = input.ReadValueU32(endianess);
			this.Padding = input.ReadValueU32(endianess);
			if (input.ReadValueU64() != 7021078959221846271UL)
			{
				throw new FormatException("Fight file doest not contain a chunk");
			}
			this.Chunk = new FightChunk(game, input, endianess);
			if (input.Position != (long)((ulong)num + (ulong)position))
			{
				throw new FormatException("Invalid chunk length");
			}
		}
		public void Serialize(PrototypeGame game, Stream output, Endian endianess)
		{
			Stream stream = new MemoryStream();
			this.SerializeFig(game, stream, endianess);
			output.WriteValueU32(4282659664U, endianess);
			output.WriteValueU32(12U, endianess);
			output.WriteValueU32((uint)(stream.Length + 24L), endianess);
			output.WriteValueU32(536872706U, endianess);
			output.WriteValueU32((uint)(stream.Length + 12L), endianess);
			output.WriteValueU32((uint)(stream.Length + 12L), endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
		}
		public void SerializeFig(PrototypeGame game, Stream output, Endian endianess)
		{
			Stream stream = new MemoryStream();
			stream.WriteString("fig0");
			stream.WriteValueU32(this.Flags, endianess);
			stream.WriteValueU32(this.Padding, endianess);
			this.Chunk.Serialize(game, stream, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
		}
		public const uint Signature = 4282659664U;
		public const uint FigDataSignature = 536872706U;
	}
}
