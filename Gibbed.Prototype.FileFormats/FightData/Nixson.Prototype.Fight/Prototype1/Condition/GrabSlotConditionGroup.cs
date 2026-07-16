using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(15277618867819471950UL)]
	public class GrabSlotConditionGroup : P1Condition
	{
		public ulong GrabSlot { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.Conditions);
		}
	}
}
