using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10169420423626653169UL)]
	public class GroundSpikeConfigureAutoOffshootsTrack : P1Track
	{
		public ulong Type { get; set; }
		public float Step { get; set; }
		public float MaxDistance { get; set; }
		public float OffsetAngleMin { get; set; }
		public float OffsetAngleMax { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Type, endianess);
			output.WriteValueF32(this.Step, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueF32(this.OffsetAngleMin, endianess);
			output.WriteValueF32(this.OffsetAngleMax, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = input.ReadValueU64(endianess);
			this.Step = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.OffsetAngleMin = input.ReadValueF32(endianess);
			this.OffsetAngleMax = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
