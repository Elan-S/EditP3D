using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.ShieldState)]
	public class ShieldStateCondition : P1Condition
	{
		public ShieldStateCondition.ShieldState State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ShieldStateCondition.ShieldState>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.State = BaseProperty.DeserializePropertyEnum<ShieldStateCondition.ShieldState>(input, endianess);
		}
		public enum ShieldState : ulong
		{
			Healing = 3944994161688763534UL,
			Broken = 1179826529873802823UL,
			Deployed = 8467737584923706070UL,
			Deploying = 6760285816536298583UL,
			Retracted = 12955520895326110976UL,
			Retracting = 9268992573163848965UL
		}
	}
}
