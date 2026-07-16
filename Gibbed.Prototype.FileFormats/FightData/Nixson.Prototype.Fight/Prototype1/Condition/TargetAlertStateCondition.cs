using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.AI)]
	[KnownCondition(ConditionHash.TargetAlertState)]
	public class TargetAlertStateCondition : P1Condition
	{
		public AIState State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<AIState>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.State = BaseProperty.DeserializePropertyEnum<AIState>(input, endianess);
		}
	}
}
