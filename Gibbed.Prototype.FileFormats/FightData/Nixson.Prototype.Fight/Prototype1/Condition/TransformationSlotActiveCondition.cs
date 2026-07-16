using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.TransformationSlotActive)]
	public class TransformationSlotActiveCondition : P1Condition
	{
		public ulong SlotName { get; set; }
		public bool IsActive { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.SlotName, endianess);
			output.WriteValueB32(this.IsActive, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SlotName = input.ReadValueU64(endianess);
			this.IsActive = input.ReadValueB32(endianess);
		}
	}
}
