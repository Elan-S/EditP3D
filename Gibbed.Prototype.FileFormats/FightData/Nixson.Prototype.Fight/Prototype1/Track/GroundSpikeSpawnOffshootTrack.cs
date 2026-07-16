using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17952288427831955926UL)]
	public class GroundSpikeSpawnOffshootTrack : P1Track
	{
		public ulong Type { get; set; }
		public int NumMin { get; set; }
		public int NumMax { get; set; }
		public bool IsForSuperSpike { get; set; }
		public float MaxArc { get; set; }
		public float AngleOffsetMin { get; set; }
		public float AngleOffsetMax { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Type, endianess);
			output.WriteValueS32(this.NumMin, endianess);
			output.WriteValueS32(this.NumMax, endianess);
			output.WriteValueB32(this.IsForSuperSpike, endianess);
			output.WriteValueF32(this.MaxArc, endianess);
			output.WriteValueF32(this.AngleOffsetMin, endianess);
			output.WriteValueF32(this.AngleOffsetMax, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = input.ReadValueU64(endianess);
			this.NumMin = input.ReadValueS32(endianess);
			this.NumMax = input.ReadValueS32(endianess);
			this.IsForSuperSpike = input.ReadValueB32(endianess);
			this.MaxArc = input.ReadValueF32(endianess);
			this.AngleOffsetMin = input.ReadValueF32(endianess);
			this.AngleOffsetMax = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
