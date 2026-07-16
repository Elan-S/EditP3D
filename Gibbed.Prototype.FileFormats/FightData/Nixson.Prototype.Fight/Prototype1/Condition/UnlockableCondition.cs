using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.Unlockable)]
	public class UnlockableCondition : P1Condition
	{
		public UnlockableEnum UnlockableType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.UnlockableType = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
		}
	}
}
