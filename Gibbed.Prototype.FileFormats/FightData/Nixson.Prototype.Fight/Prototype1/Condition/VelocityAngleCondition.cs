using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.VelocityAngle)]
	public class VelocityAngleCondition : P1Condition
	{
		public VelocityOriginType Type { get; set; }
		public CompareOperator CompareTime { get; set; }
		public float Angle { get; set; }
		public bool XZ { get; set; }
		public bool Signed { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<VelocityOriginType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueB32(this.XZ, endianess);
			output.WriteValueB32(this.Signed, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<VelocityOriginType>(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.XZ = input.ReadValueB32(endianess);
			this.Signed = input.ReadValueB32(endianess);
		}
	}
}
