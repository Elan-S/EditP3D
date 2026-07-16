using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(10129924824190329982UL)]
	public class GrabTargetAngleCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public float Angle { get; set; }
		public bool UseMovementDirection { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueB32(this.UseMovementDirection, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.UseMovementDirection = input.ReadValueB32(endianess);
		}
	}
}
