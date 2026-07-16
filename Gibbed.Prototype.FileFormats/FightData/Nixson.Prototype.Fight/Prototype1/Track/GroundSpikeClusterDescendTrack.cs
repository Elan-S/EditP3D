using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.GroundSpikeClusterDescend)]
	public class GroundSpikeClusterDescendTrack : P1Track
	{
		public bool ApplyToOffshoots { get; set; }
		public float DescendDuration { get; set; }
		public float DescendDelayMin { get; set; }
		public float DescendDelayMax { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.ApplyToOffshoots, endianess);
			output.WriteValueF32(this.DescendDuration, endianess);
			output.WriteValueF32(this.DescendDelayMin, endianess);
			output.WriteValueF32(this.DescendDelayMax, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ApplyToOffshoots = input.ReadValueB32(endianess);
			this.DescendDuration = input.ReadValueF32(endianess);
			this.DescendDelayMin = input.ReadValueF32(endianess);
			this.DescendDelayMax = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
