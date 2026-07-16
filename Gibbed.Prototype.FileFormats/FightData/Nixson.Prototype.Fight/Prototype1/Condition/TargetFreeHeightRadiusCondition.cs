using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.TargetFreeHeightRadius)]
	public class TargetFreeHeightRadiusCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public float Radius { get; set; }
		public float HeightTolerance { get; set; }
		public bool GroundUnit { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.HeightTolerance, endianess);
			output.WriteValueB32(this.GroundUnit, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.HeightTolerance = input.ReadValueF32(endianess);
			this.GroundUnit = input.ReadValueB32(endianess);
		}
	}
}
