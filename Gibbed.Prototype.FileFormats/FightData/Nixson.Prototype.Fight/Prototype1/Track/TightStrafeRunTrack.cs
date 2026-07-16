using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11201360559034481856UL)]
	public class TightStrafeRunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Distance { get; set; }
		public float TurnRate { get; set; }
		public float MinSpeed { get; set; }
		public float MaxSpeed { get; set; }
		public float DistanceTolerance { get; set; }
		public float HeightTolerance { get; set; }
		public float ExtraHeightStart { get; set; }
		public float LengthPostStart { get; set; }
		public float FreeRadiusEnd { get; set; }
		public bool GoBack { get; set; }
		public bool UseTargetIntermediate { get; set; }
		public bool GoUpSoon { get; set; }
		public float HeightAtTarget { get; set; }
		public LookAtType TemporaryTarget { get; set; }
		public Vector SpeedFactor { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Distance, endianess);
			output.WriteValueF32(this.TurnRate, endianess);
			output.WriteValueF32(this.MinSpeed, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			output.WriteValueF32(this.DistanceTolerance, endianess);
			output.WriteValueF32(this.HeightTolerance, endianess);
			output.WriteValueF32(this.ExtraHeightStart, endianess);
			output.WriteValueF32(this.LengthPostStart, endianess);
			output.WriteValueF32(this.FreeRadiusEnd, endianess);
			output.WriteValueB32(this.GoBack, endianess);
			output.WriteValueB32(this.UseTargetIntermediate, endianess);
			output.WriteValueB32(this.GoUpSoon, endianess);
			output.WriteValueF32(this.HeightAtTarget, endianess);
			BaseProperty.SerializePropertyEnum<LookAtType>(output, endianess, this.TemporaryTarget);
			this.SpeedFactor.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.TurnRate = input.ReadValueF32(endianess);
			this.MinSpeed = input.ReadValueF32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.DistanceTolerance = input.ReadValueF32(endianess);
			this.HeightTolerance = input.ReadValueF32(endianess);
			this.ExtraHeightStart = input.ReadValueF32(endianess);
			this.LengthPostStart = input.ReadValueF32(endianess);
			this.FreeRadiusEnd = input.ReadValueF32(endianess);
			this.GoBack = input.ReadValueB32(endianess);
			this.UseTargetIntermediate = input.ReadValueB32(endianess);
			this.GoUpSoon = input.ReadValueB32(endianess);
			this.HeightAtTarget = input.ReadValueF32(endianess);
			this.TemporaryTarget = BaseProperty.DeserializePropertyEnum<LookAtType>(input, endianess);
			this.SpeedFactor.Deserialize(input, endianess);
		}
	}
}
