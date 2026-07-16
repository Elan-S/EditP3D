using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11338020713353378177UL)]
	public class TargetDefenseCondition : P1Condition
	{
		public TargetDefenseCondition.DefenseType Defense { get; set; }
		public CompareOperator Compare { get; set; }
		public float Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<TargetDefenseCondition.DefenseType>(output, endianess, this.Defense);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Defense = BaseProperty.DeserializePropertyEnum<TargetDefenseCondition.DefenseType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Value = input.ReadValueF32(endianess);
		}
		public enum DefenseType : ulong
		{
			Armor = 4585032012356833025UL,
			Block = 4693892474936023823UL
		}
	}
}
