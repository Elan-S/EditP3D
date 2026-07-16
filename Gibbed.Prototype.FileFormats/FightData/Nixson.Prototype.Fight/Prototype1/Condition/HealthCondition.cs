using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.Health)]
	public class HealthCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public float HealthThreshold { get; set; }
		public bool UsePercentage { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.HealthThreshold, endianess);
			output.WriteValueB32(this.UsePercentage, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.HealthThreshold = input.ReadValueF32(endianess);
			this.UsePercentage = input.ReadValueB32(endianess);
		}
	}
}
