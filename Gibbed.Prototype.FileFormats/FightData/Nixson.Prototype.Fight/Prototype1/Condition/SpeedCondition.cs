using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Speed)]
	public class SpeedCondition : P1Condition
	{
		public CompareOperator Since { get; set; }
		public float Speed { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Since);
			output.WriteValueF32(this.Speed, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Since = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Speed = input.ReadValueF32(endianess);
		}
	}
}
