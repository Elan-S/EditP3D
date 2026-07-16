using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.JumpFromWater)]
	public class JumpFromWaterTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float FlightTimeMin { get; set; }
		public float FlightTimeMax { get; set; }
		public float UpOvershootDistance { get; set; }
		public float ForwardDistanceMin { get; set; }
		public float ForwardDistanceMax { get; set; }
		public float TurningVelocityMin { get; set; }
		public float TurningVelocityMinForwardSpeedCap { get; set; }
		public float TurningVelocityMax { get; set; }
		public float TurningVelocityMaxForwardSpeedCap { get; set; }
		public float MaxFallSpeed { get; set; }
		public float ForwardVelocityScale { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.FlightTimeMin, endianess);
			output.WriteValueF32(this.FlightTimeMax, endianess);
			output.WriteValueF32(this.UpOvershootDistance, endianess);
			output.WriteValueF32(this.ForwardDistanceMin, endianess);
			output.WriteValueF32(this.ForwardDistanceMax, endianess);
			output.WriteValueF32(this.TurningVelocityMin, endianess);
			output.WriteValueF32(this.TurningVelocityMinForwardSpeedCap, endianess);
			output.WriteValueF32(this.TurningVelocityMax, endianess);
			output.WriteValueF32(this.TurningVelocityMaxForwardSpeedCap, endianess);
			output.WriteValueF32(this.MaxFallSpeed, endianess);
			output.WriteValueF32(this.ForwardVelocityScale, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.FlightTimeMin = input.ReadValueF32(endianess);
			this.FlightTimeMax = input.ReadValueF32(endianess);
			this.UpOvershootDistance = input.ReadValueF32(endianess);
			this.ForwardDistanceMin = input.ReadValueF32(endianess);
			this.ForwardDistanceMax = input.ReadValueF32(endianess);
			this.TurningVelocityMin = input.ReadValueF32(endianess);
			this.TurningVelocityMinForwardSpeedCap = input.ReadValueF32(endianess);
			this.TurningVelocityMax = input.ReadValueF32(endianess);
			this.TurningVelocityMaxForwardSpeedCap = input.ReadValueF32(endianess);
			this.MaxFallSpeed = input.ReadValueF32(endianess);
			this.ForwardVelocityScale = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
