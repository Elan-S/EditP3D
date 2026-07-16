using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.WhipFistRetract)]
	public class WhipFistRetractTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Velocity { get; set; }
		public float SoundLeadTime { get; set; }
		public int MinSegmentsRemaining { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Velocity, endianess);
			output.WriteValueF32(this.SoundLeadTime, endianess);
			output.WriteValueS32(this.MinSegmentsRemaining, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Velocity = input.ReadValueF32(endianess);
			this.SoundLeadTime = input.ReadValueF32(endianess);
			this.MinSegmentsRemaining = input.ReadValueS32(endianess);
		}
	}
}
