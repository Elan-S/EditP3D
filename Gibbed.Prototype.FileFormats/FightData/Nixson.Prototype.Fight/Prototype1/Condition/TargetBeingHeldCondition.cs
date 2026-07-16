using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.TargetBeingHeld)]
	public class TargetBeingHeldCondition : P1Condition
	{
		public bool Held { get; set; }
		public bool DontCountIfHeldByMe { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Held, endianess);
			output.WriteValueB32(this.DontCountIfHeldByMe, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Held = input.ReadValueB32(endianess);
			this.DontCountIfHeldByMe = input.ReadValueB32(endianess);
		}
	}
}
