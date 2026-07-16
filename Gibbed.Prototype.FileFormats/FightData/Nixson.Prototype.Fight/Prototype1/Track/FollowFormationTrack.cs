using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.FollowFormation)]
	public class FollowFormationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float TurningVelocity { get; set; }
		public float Acceleration { get; set; }
		public float BrakingDeceleration { get; set; }
		public float ExtraBrakingDistance { get; set; }
		public float LookAhead { get; set; }
		public float MaxVelocity { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.TurningVelocity, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.BrakingDeceleration, endianess);
			output.WriteValueF32(this.ExtraBrakingDistance, endianess);
			output.WriteValueF32(this.LookAhead, endianess);
			output.WriteValueF32(this.MaxVelocity, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.Acceleration = input.ReadValueF32(endianess);
			this.BrakingDeceleration = input.ReadValueF32(endianess);
			this.ExtraBrakingDistance = input.ReadValueF32(endianess);
			this.LookAhead = input.ReadValueF32(endianess);
			this.MaxVelocity = input.ReadValueF32(endianess);
		}
	}
}
