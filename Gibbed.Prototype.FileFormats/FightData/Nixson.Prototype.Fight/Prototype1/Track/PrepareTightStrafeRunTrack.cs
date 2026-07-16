using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13295184067467123021UL)]
	public class PrepareTightStrafeRunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinSpeed { get; set; }
		public float MaxSpeed { get; set; }
		public float DistanceTolerance { get; set; }
		public float HeightTolerance { get; set; }
		public float ExtraHeightStart { get; set; }
		public float LengthPostStart { get; set; }
		public float WaitTimeAtStart { get; set; }
		public float HeightAtTarget { get; set; }
		public Vector SpeedFactor { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MinSpeed, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			output.WriteValueF32(this.DistanceTolerance, endianess);
			output.WriteValueF32(this.HeightTolerance, endianess);
			output.WriteValueF32(this.ExtraHeightStart, endianess);
			output.WriteValueF32(this.LengthPostStart, endianess);
			output.WriteValueF32(this.WaitTimeAtStart, endianess);
			output.WriteValueF32(this.HeightAtTarget, endianess);
			this.SpeedFactor.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MinSpeed = input.ReadValueF32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.DistanceTolerance = input.ReadValueF32(endianess);
			this.HeightTolerance = input.ReadValueF32(endianess);
			this.ExtraHeightStart = input.ReadValueF32(endianess);
			this.LengthPostStart = input.ReadValueF32(endianess);
			this.WaitTimeAtStart = input.ReadValueF32(endianess);
			this.HeightAtTarget = input.ReadValueF32(endianess);
			this.SpeedFactor.Deserialize(input, endianess);
		}
	}
}
