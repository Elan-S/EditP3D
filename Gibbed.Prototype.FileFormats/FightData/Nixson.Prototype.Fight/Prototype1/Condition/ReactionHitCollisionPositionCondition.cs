using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(18412756136391605373UL)]
	public class ReactionHitCollisionPositionCondition : P1Condition
	{
		public PositionType Component { get; set; }
		public CompareOperator Compare { get; set; }
		public float Position { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<PositionType>(output, endianess, this.Component);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Position, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Component = BaseProperty.DeserializePropertyEnum<PositionType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Position = input.ReadValueF32(endianess);
		}
	}
}
