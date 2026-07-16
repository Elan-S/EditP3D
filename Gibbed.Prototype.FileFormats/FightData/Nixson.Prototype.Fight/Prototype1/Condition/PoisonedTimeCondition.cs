using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(17568129308589311056UL)]
	public class PoisonedTimeCondition : P1Condition
	{
		public CompareOperator CompareTime { get; set; }
		public float Time { get; set; }
		public int PoisonType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Time, endianess);
			output.WriteValueS32(this.PoisonType, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Time = input.ReadValueF32(endianess);
			this.PoisonType = input.ReadValueS32(endianess);
		}
	}
}
