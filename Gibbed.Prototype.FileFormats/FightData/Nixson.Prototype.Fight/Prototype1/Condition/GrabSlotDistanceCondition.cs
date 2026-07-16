using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.GrabSlotDistance)]
	public class GrabSlotDistanceCondition : P1Condition
	{
		public ulong GrabSlot { get; set; }
		public DirectionType Direction { get; set; }
		public CompareOperator Compare { get; set; }
		public float Distance { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			BaseProperty.SerializePropertyEnum<DirectionType>(output, endianess, this.Direction);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Distance, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Direction = BaseProperty.DeserializePropertyEnum<DirectionType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Distance = input.ReadValueF32(endianess);
		}
	}
}
