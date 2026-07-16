using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11178274967742530991UL)]
	public class TurnInputCondition : P1Condition
	{
		public TurnInputCondition.PreviourDirectionType PreviousDirection { get; set; }
		public float MinAngle { get; set; }
		public float MaxAngle { get; set; }
		public TurnInputCondition.DirectionRangeType RangeType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<TurnInputCondition.PreviourDirectionType>(output, endianess, this.PreviousDirection);
			output.WriteValueF32(this.MinAngle, endianess);
			output.WriteValueF32(this.MaxAngle, endianess);
			BaseProperty.SerializePropertyEnum<TurnInputCondition.DirectionRangeType>(output, endianess, this.RangeType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.PreviousDirection = BaseProperty.DeserializePropertyEnum<TurnInputCondition.PreviourDirectionType>(input, endianess);
			this.MinAngle = input.ReadValueF32(endianess);
			this.MaxAngle = input.ReadValueF32(endianess);
			this.RangeType = BaseProperty.DeserializePropertyEnum<TurnInputCondition.DirectionRangeType>(input, endianess);
		}
		public enum PreviourDirectionType : ulong
		{
			FacingDirection = 6813351740423163961UL,
			PhysicalVelocityDirection = 9314445876697039051UL
		}
		public enum DirectionRangeType : ulong
		{
			InsideRange = 16608783493572752571UL,
			OutsideRange = 5936946687786899414UL
		}
	}
}
