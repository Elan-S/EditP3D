using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.RagdollPossible)]
	public class RagdollPossibleCondition : P1Condition
	{
		public bool RagdollAlreadyCreated { get; set; }
		public bool Possible { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.RagdollAlreadyCreated, endianess);
			output.WriteValueB32(this.Possible, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.RagdollAlreadyCreated = input.ReadValueB32(endianess);
			this.Possible = input.ReadValueB32(endianess);
		}
	}
}
