using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.AutoTargetPlayer)]
	public class AutoTargetPlayerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool SetTarget { get; set; }
		public float Arc { get; set; }
		public float MaxDistance { get; set; }
		public bool KeepChecking { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.SetTarget, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueB32(this.KeepChecking, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.SetTarget = input.ReadValueB32(endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.KeepChecking = input.ReadValueB32(endianess);
		}
	}
}
