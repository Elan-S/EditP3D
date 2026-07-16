using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	public class EBrakeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinTimeDelay { get; set; }
		public float MaxTimeDelay { get; set; }
		public float TurningSpeed { get; set; }
		public float Braking { get; set; }
		public float TurningAmount { get; set; }
		public float SkidSeverity { get; set; }
		public BranchReference MyBranch { get; set; } = new BranchReference();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MinTimeDelay, endianess);
			output.WriteValueF32(this.MaxTimeDelay, endianess);
			output.WriteValueF32(this.TurningSpeed, endianess);
			output.WriteValueF32(this.Braking, endianess);
			output.WriteValueF32(this.TurningAmount, endianess);
			output.WriteValueF32(this.SkidSeverity, endianess);
			this.MyBranch.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MinTimeDelay = input.ReadValueF32(endianess);
			this.MaxTimeDelay = input.ReadValueF32(endianess);
			this.TurningSpeed = input.ReadValueF32(endianess);
			this.Braking = input.ReadValueF32(endianess);
			this.TurningAmount = input.ReadValueF32(endianess);
			this.SkidSeverity = input.ReadValueF32(endianess);
			this.MyBranch.Deserialize(input, endianess);
		}
	}
}
