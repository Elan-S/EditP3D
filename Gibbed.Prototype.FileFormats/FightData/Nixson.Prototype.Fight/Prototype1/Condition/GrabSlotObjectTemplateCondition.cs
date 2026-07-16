using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(16246123553012195093UL)]
	public class GrabSlotObjectTemplateCondition : P1Condition
	{
		public ulong GrabSlot { get; set; }
		public CompareOperator Compare { get; set; }
		public ulong ObjectTemplate { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueU64(this.ObjectTemplate, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.ObjectTemplate = input.ReadValueU64(endianess);
		}
	}
}
