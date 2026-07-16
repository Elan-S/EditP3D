using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(15341108296873221587UL)]
	public class HunterShortRangeHopCornerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityRunUp { get; set; }
		public float VelocityHorizontal { get; set; }
		public float Gravity { get; set; }
		public float OrientationBlendTime { get; set; }
		public float MaxFallSpeed { get; set; }
		public ulong NewSupportingLimb { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityRunUp, endianess);
			output.WriteValueF32(this.VelocityHorizontal, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.OrientationBlendTime, endianess);
			output.WriteValueF32(this.MaxFallSpeed, endianess);
			output.WriteValueU64(this.NewSupportingLimb, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityRunUp = input.ReadValueF32(endianess);
			this.VelocityHorizontal = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.OrientationBlendTime = input.ReadValueF32(endianess);
			this.MaxFallSpeed = input.ReadValueF32(endianess);
			this.NewSupportingLimb = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
