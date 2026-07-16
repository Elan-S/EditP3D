using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(11320541242804651096UL)]
	public class TimerCompareCondition : P1Condition
	{
		public ulong Timer { get; set; }
		public CompareOperator Compare { get; set; }
		public float Time { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Timer, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Time, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Timer = input.ReadValueU64(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Time = input.ReadValueF32(endianess);
		}
	}
}
