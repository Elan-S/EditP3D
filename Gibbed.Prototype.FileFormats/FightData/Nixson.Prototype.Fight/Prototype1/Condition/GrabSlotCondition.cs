using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.GrabSlot)]
	public class GrabSlotCondition : P1Condition
	{
		public ulong GrabSlotHash { get; set; }
		public bool IsGrabbing { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlotHash, endianess);
			output.WriteValueB32(this.IsGrabbing);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabSlotHash = input.ReadValueU64(endianess);
			this.IsGrabbing = input.ReadValueB32(endianess);
		}
	}
}
