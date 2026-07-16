using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.WallExit)]
	public class WallExitTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong SupportingLimb { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float Gravity { get; set; }
		public float WallAngleUp { get; set; }
		public float WallAngleLateral { get; set; }
		public float WallAngleDown { get; set; }
		public float TurningVelocity { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.SupportingLimb, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.WallAngleUp, endianess);
			output.WriteValueF32(this.WallAngleLateral, endianess);
			output.WriteValueF32(this.WallAngleDown, endianess);
			output.WriteValueF32(this.TurningVelocity, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.SupportingLimb = input.ReadValueU64(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.WallAngleUp = input.ReadValueF32(endianess);
			this.WallAngleLateral = input.ReadValueF32(endianess);
			this.WallAngleDown = input.ReadValueF32(endianess);
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
