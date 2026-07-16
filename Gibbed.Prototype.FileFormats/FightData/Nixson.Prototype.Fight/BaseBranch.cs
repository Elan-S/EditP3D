using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight
{
	public class BaseBranch : FightNode
	{
		public ulong NameHash { get; set; }
		public string Path { get; set; } = "";
		public uint NumberOfSiblings { get; set; }
		public List<BaseBranch> Branches { get; set; } = new List<BaseBranch>();
		public override object Clone(PrototypeGame game)
		{
			Stream stream = new MemoryStream();
			BaseBranch.SerializeBaseBranch(game, stream, Endian.Little, this, 0);
			stream.Position = 0L;
			ulong hash = stream.ReadValueU64();
			return BaseBranch.DeserializeBaseBranch(game, stream, Endian.Little, hash);
		}
		public override void Serialize(Stream output, Endian endianess)
		{
			output.WriteValueU64(this.NameHash, endianess);
			output.WriteStringAlignedU32(this.Path, endianess);
			output.WriteValueU32(this.NumberOfSiblings, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			this.NameHash = input.ReadValueU64(endianess);
			this.Path = input.ReadStringAlignedU32(endianess);
			this.NumberOfSiblings = input.ReadValueU32(endianess);
		}
		public static void SerializeBaseBranch(PrototypeGame game, Stream output, Endian endianess, BaseBranch branch, int numSiblings)
		{
			if (game == PrototypeGame.P1)
			{
				BaseBranch.P1_SerializeBaseBranch(output, endianess, branch, numSiblings);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			BaseBranch.P2_SerializeBaseBranch(output, endianess, branch, numSiblings);
		}
		public static void SerializeBaseBranches(PrototypeGame game, Stream output, Endian endianess, List<BaseBranch> branches)
		{
			if (game == PrototypeGame.P1)
			{
				BaseBranch.P1_SerializeBaseBranches(output, endianess, branches);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			BaseBranch.P2_SerializeBaseBranches(output, endianess, branches);
		}
		public static BaseBranch DeserializeBaseBranch(PrototypeGame game, Stream input, Endian endianess, ulong hash)
		{
			if (game == PrototypeGame.P1)
			{
				return BaseBranch.P1_DeserializeBaseBranch(input, endianess, hash);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return BaseBranch.P2_DeserializeBaseBranch(input, endianess, hash);
		}
		public static List<BaseBranch> DeserializeBaseBranches(PrototypeGame game, Stream input, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				return BaseBranch.P1_DeserializeBaseBranches(input, endianess);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return BaseBranch.P2_DeserializeBaseBranches(input, endianess);
		}
		public static void P1_SerializeBaseBranch(Stream output, Endian endianess, BaseBranch branch, int numSiblings)
		{
			Stream stream = new MemoryStream();
			branch.NumberOfSiblings = (uint)numSiblings;
			branch.Serialize(stream, endianess);
			output.WriteValueU64(branch.TypeHash, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			branch.SerializeProperties(PrototypeGame.P1, output, endianess);
			BaseBranch.P1_SerializeBaseBranches(output, endianess, branch.Branches);
			output.WriteValueU64(0UL);
		}
		public static void P1_SerializeBaseBranches(Stream output, Endian endianess, List<BaseBranch> branches)
		{
			foreach (BaseBranch branch in branches)
			{
				BaseBranch.P1_SerializeBaseBranch(output, endianess, branch, branches.Count);
			}
		}
		public static BaseBranch P1_DeserializeBaseBranch(Stream input, Endian endianess, ulong hash)
		{
			BaseBranch baseBranch = Factory<BaseBranch, KnownBranchAttribute>.Build(PrototypeGame.P1, hash);
			if (baseBranch == null)
			{
				throw new NotImplementedException("Unknown branch");
			}
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			baseBranch.Deserialize(input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid branch length");
			}
			baseBranch.DeserializeProperties(PrototypeGame.P1, input, endianess);
			baseBranch.Branches = BaseBranch.DeserializeBaseBranches(PrototypeGame.P1, input, endianess);
			input.ReadValueU64(Endian.Little);
			return baseBranch;
		}
		public static List<BaseBranch> P1_DeserializeBaseBranches(Stream input, Endian endianess)
		{
			List<BaseBranch> list = new List<BaseBranch>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				list.Add(BaseBranch.P1_DeserializeBaseBranch(input, endianess, num));
			}
			input.Position -= 8L;
			return list;
		}
		private static void P2_SerializeBaseBranch(Stream output, Endian endianess, BaseBranch branch, int numSiblings)
		{
			Stream stream = new MemoryStream();
			branch.NumberOfSiblings = (uint)numSiblings;
			branch.Serialize(stream, endianess);
			output.WriteValueU64(branch.TypeHash, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			branch.SerializeProperties(PrototypeGame.P1, output, endianess);
			BaseBranch.P2_SerializeBaseBranches(output, endianess, branch.Branches);
			output.WriteValueU64(0UL);
		}
		private static void P2_SerializeBaseBranches(Stream output, Endian endianess, List<BaseBranch> branches)
		{
			foreach (BaseBranch branch in branches)
			{
				BaseBranch.P2_SerializeBaseBranch(output, endianess, branch, branches.Count);
			}
		}
		private static BaseBranch P2_DeserializeBaseBranch(Stream input, Endian endianess, ulong hash)
		{
			BaseBranch baseBranch = Factory<BaseBranch, KnownBranchAttribute>.Build(PrototypeGame.P2, hash);
			if (baseBranch == null)
			{
				throw new NotImplementedException("Unknown branch");
			}
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			baseBranch.Deserialize(input, endianess);
			baseBranch.DeserializeProperties(PrototypeGame.P2, input, endianess);
			baseBranch.Branches = BaseBranch.P2_DeserializeBaseBranches(input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid branch length");
			}
			input.ReadValueU64(Endian.Little);
			return baseBranch;
		}
		private static List<BaseBranch> P2_DeserializeBaseBranches(Stream input, Endian endianess)
		{
			List<BaseBranch> list = new List<BaseBranch>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				list.Add(BaseBranch.P2_DeserializeBaseBranch(input, endianess, num));
			}
			input.Position -= 8L;
			return list;
		}
	}
}
