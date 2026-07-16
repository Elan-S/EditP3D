using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.FlyingAttack)]
	public class FlyingAttackTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEndMin { get; set; }
		public float TimeEndMax { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float AccelerationMin { get; set; }
		public float AccelerationMax { get; set; }
		public float TrackingMin { get; set; }
		public float TrackingMax { get; set; }
		public bool FaceDirection { get; set; }
		public bool UseAutoTarget { get; set; }
		public bool SlideOnGround { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEndMin, endianess);
			output.WriteValueF32(this.TimeEndMax, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.AccelerationMin, endianess);
			output.WriteValueF32(this.AccelerationMax, endianess);
			output.WriteValueF32(this.TrackingMin, endianess);
			output.WriteValueF32(this.TrackingMax, endianess);
			output.WriteValueB32(this.FaceDirection, endianess);
			output.WriteValueB32(this.UseAutoTarget, endianess);
			output.WriteValueB32(this.SlideOnGround, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEndMin = input.ReadValueF32(endianess);
			this.TimeEndMax = input.ReadValueF32(endianess);
			this.Direction = new Vector(input, endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.AccelerationMin = input.ReadValueF32(endianess);
			this.AccelerationMax = input.ReadValueF32(endianess);
			this.TrackingMin = input.ReadValueF32(endianess);
			this.TrackingMax = input.ReadValueF32(endianess);
			this.FaceDirection = input.ReadValueB32(endianess);
			this.UseAutoTarget = input.ReadValueB32(endianess);
			this.SlideOnGround = input.ReadValueB32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
