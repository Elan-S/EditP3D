using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownCondition(16953438673011561868UL)]
	public class EventMotionStateCondition : P1Condition
	{
		public MovementMotionState State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<MovementMotionState>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.State = BaseProperty.DeserializePropertyEnum<MovementMotionState>(input, endianess);
		}
	}
}
