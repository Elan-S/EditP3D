using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.SupportingLimbIsActive)]
	public class SupportingLimbIsActiveCondition : P1Condition
	{
		public ulong SupportingLimbHash { get; set; }
		public bool IsActive { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.SupportingLimbHash, endianess);
			output.WriteValueB32(this.IsActive, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SupportingLimbHash = input.ReadValueU64(endianess);
			this.IsActive = input.ReadValueB32(endianess);
		}
	}
}
