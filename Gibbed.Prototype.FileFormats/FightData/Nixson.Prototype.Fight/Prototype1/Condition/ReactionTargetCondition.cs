using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.ReactionTarget)]
	public class ReactionTargetCondition : P1Condition
	{
		public bool DoesMatch { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.DoesMatch, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.DoesMatch = input.ReadValueB32(endianess);
		}
	}
}
