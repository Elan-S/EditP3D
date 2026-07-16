using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight
{
	public class FightChunk
	{
		public uint Unknown1 { get; set; }
		public ulong NameHash { get; set; }
		public ContextHash Context { get; set; }
		public BranchReference BranchRef { get; set; }
		public List<BaseBranch> Branches { get; set; }
		public FightChunk()
		{
		}
		public FightChunk(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Deserialize(game, input, endianess);
		}
		private void P1_Serialize(Stream output, Endian endianess)
		{
			output.WriteValueU64(7021078959221846271UL);
			Stream stream = new MemoryStream();
			stream.WriteValueU32(this.Unknown1, endianess);
			stream.WriteValueU64(this.NameHash, endianess);
			stream.WriteValueU64((ulong)this.Context, endianess);
			this.BranchRef.Serialize(stream, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			BaseBranch.SerializeBaseBranches(PrototypeGame.P1, output, endianess, this.Branches);
			output.WriteValueU64(0UL);
		}
		private void P1_Deserialize(Stream input, Endian endianess)
		{
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			this.Unknown1 = input.ReadValueU32(endianess);
			this.NameHash = input.ReadValueU64(endianess);
			ulong num2 = input.ReadValueU64(endianess);
			this.Context = (ContextHash)num2;
			this.BranchRef = new BranchReference(input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid chunk header length");
			}
			this.Branches = BaseBranch.DeserializeBaseBranches(PrototypeGame.P1, input, endianess);
			input.ReadValueU64(Endian.Little);
		}
		private void P2_Serialize(Stream output, Endian endianess)
		{
			throw new NotImplementedException();
		}
		private void P2_Deserialize(Stream input, Endian endianess)
		{
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			this.Unknown1 = input.ReadValueU32(endianess);
			this.NameHash = input.ReadValueU64(endianess);
			ulong num2 = input.ReadValueU64(endianess);
			this.Context = (ContextHash)num2;
			this.BranchRef = new BranchReference(input, endianess);
			input.ReadValueS32(endianess);
			this.Branches = BaseBranch.DeserializeBaseBranches(PrototypeGame.P2, input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid chunk length");
			}
			input.ReadValueU64(Endian.Little);
		}
		public void Serialize(PrototypeGame game, Stream output, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				this.P1_Serialize(output, endianess);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			this.P2_Serialize(output, endianess);
		}
		public void Deserialize(PrototypeGame game, Stream input, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				this.P1_Deserialize(input, endianess);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			this.P2_Deserialize(input, endianess);
		}
	}
}
