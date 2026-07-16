using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.ReactionGiverIsBeingGrabbed)]
	public class ReactionGiverIsBeingGrabbedCondition : P1Condition
	{
		public GrabType GrabType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<GrabType>(output, endianess, this.GrabType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabType = BaseProperty.DeserializePropertyEnum<GrabType>(input, endianess);
		}
	}
}
