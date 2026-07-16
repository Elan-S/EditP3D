using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.PedestrianAngleToTarget)]
	public class PedestrianAngleToTargetCondition : P1Condition
	{
		public CompareOperator CompareTime { get; set; }
		public float Angle { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Angle, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
		}
	}
}
