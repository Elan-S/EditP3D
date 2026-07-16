using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(15893711816904805024UL)]
	public class ChargeCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public float Charge { get; set; }
		public ChargeType Type { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Charge, endianess);
			BaseProperty.SerializePropertyEnum<ChargeType>(output, endianess, this.Type);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Charge = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<ChargeType>(input, endianess);
		}
	}
}
