using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(17412521336193876618UL)]
	public class FlyingBrakeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityXZ { get; set; }
		public float Gravity { get; set; }
		public float GravityUp { get; set; }
		public float TerminalVelocity { get; set; }
		public float VelocityUp { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityXZ, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.GravityUp, endianess);
			output.WriteValueF32(this.TerminalVelocity, endianess);
			output.WriteValueF32(this.VelocityUp, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityXZ = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.GravityUp = input.ReadValueF32(endianess);
			this.TerminalVelocity = input.ReadValueF32(endianess);
			this.VelocityUp = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
