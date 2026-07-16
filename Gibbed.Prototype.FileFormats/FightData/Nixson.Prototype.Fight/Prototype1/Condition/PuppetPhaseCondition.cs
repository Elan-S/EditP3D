using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(13689913255888844999UL)]
	public class PuppetPhaseCondition : P1Condition
	{
		public PuppetPhaseCondition.RangeType PositionConditionType { get; set; }
		public float MinPositionPhase { get; set; }
		public float MaxPositionPhase { get; set; }
		public PuppetPhaseCondition.RangeType VelocityConditionType { get; set; }
		public float MinVelocityPhase { get; set; }
		public float MaxVelocityPhase { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<PuppetPhaseCondition.RangeType>(output, endianess, this.PositionConditionType);
			output.WriteValueF32(this.MinPositionPhase, endianess);
			output.WriteValueF32(this.MaxPositionPhase, endianess);
			BaseProperty.SerializePropertyEnum<PuppetPhaseCondition.RangeType>(output, endianess, this.VelocityConditionType);
			output.WriteValueF32(this.MinVelocityPhase, endianess);
			output.WriteValueF32(this.MaxVelocityPhase, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.PositionConditionType = BaseProperty.DeserializePropertyEnum<PuppetPhaseCondition.RangeType>(input, endianess);
			this.MinPositionPhase = input.ReadValueF32(endianess);
			this.MaxPositionPhase = input.ReadValueF32(endianess);
			this.VelocityConditionType = BaseProperty.DeserializePropertyEnum<PuppetPhaseCondition.RangeType>(input, endianess);
			this.MinVelocityPhase = input.ReadValueF32(endianess);
			this.MaxVelocityPhase = input.ReadValueF32(endianess);
		}
		public enum RangeType : ulong
		{
			InsideRange = 16608783493572752571UL,
			OutsideRange = 5936946687786899414UL
		}
	}
}
