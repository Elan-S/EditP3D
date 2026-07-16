using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.VelocityAngular)]
	public class VelocityAngularCondition : P1Condition
	{
		public VelocityOriginType Type { get; set; }
		public DirectionType Direction { get; set; }
		public CompareOperator Compare { get; set; }
		public float AngularVelocity { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<VelocityOriginType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyEnum<DirectionType>(output, endianess, this.Direction);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.AngularVelocity, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<VelocityOriginType>(input, endianess);
			this.Direction = BaseProperty.DeserializePropertyEnum<DirectionType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.AngularVelocity = input.ReadValueF32(endianess);
		}
	}
}
