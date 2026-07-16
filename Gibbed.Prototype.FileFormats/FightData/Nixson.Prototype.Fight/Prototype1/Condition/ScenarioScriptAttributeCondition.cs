using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(15810646280967156986UL)]
	public class ScenarioScriptAttributeCondition : P1Condition
	{
		public ulong GameObjectSlot { get; set; }
		public GameObjectAttributeType Type { get; set; }
		public string Attributekey { get; set; }
		public string Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GameObjectSlot, endianess);
			BaseProperty.SerializePropertyEnum<GameObjectAttributeType>(output, endianess, this.Type);
			output.WriteStringAlignedU32(this.Attributekey, endianess);
			output.WriteStringAlignedU32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GameObjectSlot = input.ReadValueU64(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<GameObjectAttributeType>(input, endianess);
			this.Attributekey = input.ReadStringAlignedU32(endianess);
			this.Value = input.ReadStringAlignedU32(endianess);
		}
	}
}
