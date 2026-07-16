using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.ReactionEvent)]
	public class ReactionEventCondition : P1Condition
	{
		public bool IsReactionEvent { get; set; }
		public ReactionType ReactionType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsReactionEvent, endianess);
			BaseProperty.SerializePropertyEnum<ReactionType>(output, endianess, this.ReactionType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsReactionEvent = input.ReadValueB32(endianess);
			this.ReactionType = BaseProperty.DeserializePropertyEnum<ReactionType>(input, endianess);
		}
	}
}
