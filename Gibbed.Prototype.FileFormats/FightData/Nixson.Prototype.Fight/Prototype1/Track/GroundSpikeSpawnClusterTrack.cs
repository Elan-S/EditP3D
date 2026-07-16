using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(16875765416745917900UL)]
	public class GroundSpikeSpawnClusterTrack : P1Track
	{
		public ulong LargeType { get; set; }
		public ulong MidType { get; set; }
		public ulong SmallType { get; set; }
		public float LargeFrac { get; set; }
		public float MidFrac { get; set; }
		public float SmallFrac { get; set; }
		public int MinShards { get; set; }
		public int MaxShards { get; set; }
		public float SpawnRadius { get; set; }
		public float SpreadRadius { get; set; }
		public float TimingRandomizerFrac { get; set; }
		public float AscendTimeMin { get; set; }
		public float AscendTimeMax { get; set; }
		public float TwistMin { get; set; }
		public float TwistMax { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.LargeType, endianess);
			output.WriteValueU64(this.MidType, endianess);
			output.WriteValueU64(this.SmallType, endianess);
			output.WriteValueF32(this.LargeFrac, endianess);
			output.WriteValueF32(this.MidFrac, endianess);
			output.WriteValueF32(this.SmallFrac, endianess);
			output.WriteValueS32(this.MinShards, endianess);
			output.WriteValueS32(this.MaxShards, endianess);
			output.WriteValueF32(this.SpawnRadius, endianess);
			output.WriteValueF32(this.SpreadRadius, endianess);
			output.WriteValueF32(this.TimingRandomizerFrac, endianess);
			output.WriteValueF32(this.AscendTimeMin, endianess);
			output.WriteValueF32(this.AscendTimeMax, endianess);
			output.WriteValueF32(this.TwistMin, endianess);
			output.WriteValueF32(this.TwistMax, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.LargeType = input.ReadValueU64(endianess);
			this.MidType = input.ReadValueU64(endianess);
			this.SmallType = input.ReadValueU64(endianess);
			this.LargeFrac = input.ReadValueF32(endianess);
			this.MidFrac = input.ReadValueF32(endianess);
			this.SmallFrac = input.ReadValueF32(endianess);
			this.MinShards = input.ReadValueS32(endianess);
			this.MaxShards = input.ReadValueS32(endianess);
			this.SpawnRadius = input.ReadValueF32(endianess);
			this.SpreadRadius = input.ReadValueF32(endianess);
			this.TimingRandomizerFrac = input.ReadValueF32(endianess);
			this.AscendTimeMin = input.ReadValueF32(endianess);
			this.AscendTimeMax = input.ReadValueF32(endianess);
			this.TwistMin = input.ReadValueF32(endianess);
			this.TwistMax = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
