using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownCondition(ConditionHash.GameObjectAttribute)]
	public class ScenarioGameObjectAttributeCondition : P1Condition
	{
		public ScenarioGameObjectSlot GameObjectSlot { get; set; }
		public string AttributeKey { get; set; }
		public ulong ValueHash { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ScenarioGameObjectSlot>(output, endianess, this.GameObjectSlot);
			output.WriteStringAlignedU32(this.AttributeKey, endianess);
			output.WriteValueU64(this.ValueHash, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GameObjectSlot = BaseProperty.DeserializePropertyEnum<ScenarioGameObjectSlot>(input, endianess);
			this.AttributeKey = input.ReadStringAlignedU32(endianess);
			this.ValueHash = input.ReadValueU64(endianess);
		}
	}
}
