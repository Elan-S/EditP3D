using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.InfectedAlerted)]
	public class InfectedAlertedCondition : P1Condition
	{
		public DirectionType Direction { get; set; }
		public CompareOperator Compare { get; set; }
		public float Distance { get; set; }
		public bool TargetingMe { get; set; }
		public bool RequireToughGuy { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<DirectionType>(output, endianess, this.Direction);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Distance, endianess);
			output.WriteValueB32(this.TargetingMe, endianess);
			output.WriteValueB32(this.RequireToughGuy, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Direction = BaseProperty.DeserializePropertyEnum<DirectionType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.TargetingMe = input.ReadValueB32(endianess);
			this.RequireToughGuy = input.ReadValueB32(endianess);
		}
	}
}
