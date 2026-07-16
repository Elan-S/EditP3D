using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.WallJump)]
	public class WallJumpTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float VelocityXZMax { get; set; }
		public float Gravity { get; set; }
		public float WallAngleMin { get; set; }
		public float WallAngleMax { get; set; }
		public float TurningVelocity { get; set; }
		public float UpThresholdArc { get; set; }
		public bool UseMomentum { get; set; }
		public float Arc { get; set; }
		public float MinYAngle { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.VelocityXZMax, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.WallAngleMin, endianess);
			output.WriteValueF32(this.WallAngleMax, endianess);
			output.WriteValueF32(this.TurningVelocity, endianess);
			output.WriteValueF32(this.UpThresholdArc, endianess);
			output.WriteValueB32(this.UseMomentum, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueF32(this.MinYAngle, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.VelocityXZMax = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.WallAngleMin = input.ReadValueF32(endianess);
			this.WallAngleMax = input.ReadValueF32(endianess);
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.UpThresholdArc = input.ReadValueF32(endianess);
			this.UseMomentum = input.ReadValueB32(endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.MinYAngle = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
