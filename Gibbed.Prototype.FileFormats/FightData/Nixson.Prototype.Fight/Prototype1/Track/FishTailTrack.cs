using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.FishTail)]
	public class FishTailTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinTimeDelay { get; set; }
		public float MaxTimeDelay { get; set; }
		public float Turning { get; set; }
		public float DesiredVelocity { get; set; }
		public float Accelration { get; set; }
		public float Braking { get; set; }
		public float SkidSeverity { get; set; }
		public float FishTailSeverity { get; set; }
		public BranchReference MyBranch { get; set; } = new BranchReference();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MinTimeDelay, endianess);
			output.WriteValueF32(this.MaxTimeDelay, endianess);
			output.WriteValueF32(this.Turning, endianess);
			output.WriteValueF32(this.DesiredVelocity, endianess);
			output.WriteValueF32(this.Accelration, endianess);
			output.WriteValueF32(this.Braking, endianess);
			output.WriteValueF32(this.SkidSeverity, endianess);
			output.WriteValueF32(this.FishTailSeverity, endianess);
			this.MyBranch.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MinTimeDelay = input.ReadValueF32(endianess);
			this.MaxTimeDelay = input.ReadValueF32(endianess);
			this.Turning = input.ReadValueF32(endianess);
			this.DesiredVelocity = input.ReadValueF32(endianess);
			this.Accelration = input.ReadValueF32(endianess);
			this.Braking = input.ReadValueF32(endianess);
			this.SkidSeverity = input.ReadValueF32(endianess);
			this.FishTailSeverity = input.ReadValueF32(endianess);
			this.MyBranch.Deserialize(input, endianess);
		}
	}
}
