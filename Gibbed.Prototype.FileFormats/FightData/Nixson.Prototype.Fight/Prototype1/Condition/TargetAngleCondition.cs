using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(10866943884008336212UL)]
	public class TargetAngleCondition : P1Condition
	{
		public CompareOperator CompareTime { get; set; }
		public float Angle { get; set; }
		public float Arc { get; set; }
		public bool UseMovementDirection { get; set; }
		public bool UseInputDirection { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueB32(this.UseMovementDirection, endianess);
			output.WriteValueB32(this.UseInputDirection, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.UseMovementDirection = input.ReadValueB32(endianess);
			this.UseInputDirection = input.ReadValueB32(endianess);
		}
	}
}
