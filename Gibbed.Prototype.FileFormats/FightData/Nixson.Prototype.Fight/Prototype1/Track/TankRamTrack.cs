using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(15946290422928976772UL)]
	public class TankRamTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float TimePreparingForward { get; set; }
		public float TimePreparingBackward { get; set; }
		public float MaxTurningSpeedFactor { get; set; }
		public float RollingScaleFactor { get; set; }
		public float RollingScaleBackwardsFactor { get; set; }
		public float RollingScaleFactorPush { get; set; }
		public float RollingScaleBackwardsFactorPush { get; set; }
		public float ToleranceForward { get; set; }
		public float Speed { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.TimePreparingForward, endianess);
			output.WriteValueF32(this.TimePreparingBackward, endianess);
			output.WriteValueF32(this.MaxTurningSpeedFactor, endianess);
			output.WriteValueF32(this.RollingScaleFactor, endianess);
			output.WriteValueF32(this.RollingScaleBackwardsFactor, endianess);
			output.WriteValueF32(this.RollingScaleFactorPush, endianess);
			output.WriteValueF32(this.RollingScaleBackwardsFactorPush, endianess);
			output.WriteValueF32(this.ToleranceForward, endianess);
			output.WriteValueF32(this.Speed, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.TimePreparingForward = input.ReadValueF32(endianess);
			this.TimePreparingBackward = input.ReadValueF32(endianess);
			this.MaxTurningSpeedFactor = input.ReadValueF32(endianess);
			this.RollingScaleFactor = input.ReadValueF32(endianess);
			this.RollingScaleBackwardsFactor = input.ReadValueF32(endianess);
			this.RollingScaleFactorPush = input.ReadValueF32(endianess);
			this.RollingScaleBackwardsFactorPush = input.ReadValueF32(endianess);
			this.ToleranceForward = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
		}
	}
}
