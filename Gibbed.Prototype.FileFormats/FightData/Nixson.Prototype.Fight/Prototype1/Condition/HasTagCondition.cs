using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownCondition(ConditionHash.HasTag)]
	public class HasTagCondition : P1Condition
	{
		public ScenarioGameObjectSlot Affiliate { get; set; }
		public string Tag { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ScenarioGameObjectSlot>(output, endianess, this.Affiliate);
			output.WriteStringAlignedU32(this.Tag, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Affiliate = BaseProperty.DeserializePropertyEnum<ScenarioGameObjectSlot>(input, endianess);
			this.Tag = input.ReadStringAlignedU32(endianess);
		}
	}
}
