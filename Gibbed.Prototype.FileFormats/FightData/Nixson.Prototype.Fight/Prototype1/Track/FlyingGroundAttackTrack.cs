using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13194223807135731504UL)]
	public class FlyingGroundAttackTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityXZMin { get; set; }
		public float VelocityXZMax { get; set; }
		public float AccelerationXZ { get; set; }
		public float Gravity { get; set; }
		public float TurningVelocity { get; set; }
		public float TargetOffset { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityXZMin, endianess);
			output.WriteValueF32(this.VelocityXZMax, endianess);
			output.WriteValueF32(this.AccelerationXZ, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.TurningVelocity, endianess);
			output.WriteValueF32(this.TargetOffset, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityXZMin = input.ReadValueF32(endianess);
			this.VelocityXZMax = input.ReadValueF32(endianess);
			this.AccelerationXZ = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.TargetOffset = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
