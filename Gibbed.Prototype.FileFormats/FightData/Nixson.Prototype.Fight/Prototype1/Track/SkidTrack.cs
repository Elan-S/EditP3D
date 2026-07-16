using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Skid)]
	public class SkidTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityInit { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float DecelerationMin { get; set; }
		public float DecelerationMax { get; set; }
		public float SprintVelocityDeceleration { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityInit, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.DecelerationMin, endianess);
			output.WriteValueF32(this.DecelerationMax, endianess);
			output.WriteValueF32(this.SprintVelocityDeceleration, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityInit = input.ReadValueF32(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.DecelerationMin = input.ReadValueF32(endianess);
			this.DecelerationMax = input.ReadValueF32(endianess);
			this.SprintVelocityDeceleration = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
