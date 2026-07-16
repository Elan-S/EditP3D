using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.MagneticBootsJump)]
	public class MagneticBootsJumpTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float FlightTime { get; set; }
		public float UpDistance { get; set; }
		public float ForwardDistance { get; set; }
		public float MaxFallSpeed { get; set; }
		public int ChoreoPriority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.FlightTime, endianess);
			output.WriteValueF32(this.UpDistance, endianess);
			output.WriteValueF32(this.ForwardDistance, endianess);
			output.WriteValueF32(this.MaxFallSpeed, endianess);
			output.WriteValueS32(this.ChoreoPriority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.FlightTime = input.ReadValueF32(endianess);
			this.UpDistance = input.ReadValueF32(endianess);
			this.ForwardDistance = input.ReadValueF32(endianess);
			this.MaxFallSpeed = input.ReadValueF32(endianess);
			this.ChoreoPriority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
