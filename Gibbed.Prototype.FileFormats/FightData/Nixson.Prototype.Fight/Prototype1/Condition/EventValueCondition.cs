using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.EventValue)]
	public class EventValueCondition : P1Condition
	{
		public ulong Event { get; set; }
		public CompareOperator Compare { get; set; }
		public int Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Event, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueS32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Event = input.ReadValueU64(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Value = input.ReadValueS32(endianess);
		}
	}
}
