using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(17885828298422195048UL)]
	public class GroundSlideTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Facing { get; set; }
		public float Deceleration { get; set; }
		public float MinInitVelocity { get; set; }
		public float MinVelocity { get; set; }
		public float MaxVelocity { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Facing, endianess);
			output.WriteValueF32(this.Deceleration, endianess);
			output.WriteValueF32(this.MinInitVelocity, endianess);
			output.WriteValueF32(this.MinVelocity, endianess);
			output.WriteValueF32(this.MaxVelocity, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Facing = input.ReadValueF32(endianess);
			this.Deceleration = input.ReadValueF32(endianess);
			this.MinInitVelocity = input.ReadValueF32(endianess);
			this.MinVelocity = input.ReadValueF32(endianess);
			this.MaxVelocity = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
