using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.Velocity)]
	public class VelocityCondition : P1Condition
	{
		public VelocityCondition.VelocityType Type { get; set; }
		public DirectionType Direction { get; set; }
		public CompareOperator Compare { get; set; }
		public float Velocity { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<VelocityCondition.VelocityType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyEnum<DirectionType>(output, endianess, this.Direction);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Velocity, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<VelocityCondition.VelocityType>(input, endianess);
			this.Direction = BaseProperty.DeserializePropertyEnum<DirectionType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Velocity = input.ReadValueF32(endianess);
		}
		public enum VelocityType : ulong
		{
			Locomotion = 16331038302739513351UL,
			Physics = 8685988000896976323UL
		}
	}
}
