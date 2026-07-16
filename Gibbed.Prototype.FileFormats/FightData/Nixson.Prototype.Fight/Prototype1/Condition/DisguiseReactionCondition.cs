using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.DisguiseReaction)]
	public class DisguiseReactionCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public AIState Reaction { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			BaseProperty.SerializePropertyEnum<AIState>(output, endianess, this.Reaction);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Reaction = BaseProperty.DeserializePropertyEnum<AIState>(input, endianess);
		}
	}
}
