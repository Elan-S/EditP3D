using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(12186319916039846263UL)]
	public class DestinationTargetOrientationCondition : P1Condition
	{
		public CompareOperator CompareTime { get; set; }
		public float Angle { get; set; }
		public bool Signed { get; set; }
		public bool UseParams { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueB32(this.Signed, endianess);
			output.WriteValueB32(this.UseParams, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.Signed = input.ReadValueB32(endianess);
			this.UseParams = input.ReadValueB32(endianess);
		}
	}
}
