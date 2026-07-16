using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Event)]
	public class EventCondition : P1Condition
	{
		public ulong Event { get; set; }
		public EventCondition.WhenType When { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Event, endianess);
			BaseProperty.SerializePropertyEnum<EventCondition.WhenType>(output, endianess, this.When);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Event = input.ReadValueU64(endianess);
			this.When = BaseProperty.DeserializePropertyEnum<EventCondition.WhenType>(input, endianess);
		}
		public enum WhenType : ulong
		{
			OnEnter = 4674110501632545327UL,
			OnExit = 10613578434616600639UL
		}
	}
}
