using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.FollowTarget)]
	public class FollowTargetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MaxAngleFromTargetFacing { get; set; }
		public float MaxMotionLengthDelta { get; set; }
		public float MaxRotationDelta { get; set; }
		public bool SmoothWithLast { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MaxAngleFromTargetFacing, endianess);
			output.WriteValueF32(this.MaxMotionLengthDelta, endianess);
			output.WriteValueF32(this.MaxRotationDelta, endianess);
			output.WriteValueB32(this.SmoothWithLast, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MaxAngleFromTargetFacing = input.ReadValueF32(endianess);
			this.MaxMotionLengthDelta = input.ReadValueF32(endianess);
			this.MaxRotationDelta = input.ReadValueF32(endianess);
			this.SmoothWithLast = input.ReadValueB32(endianess);
		}
	}
}
