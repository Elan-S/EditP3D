using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(16570927403630476524UL)]
	public class SupportingLimbTestSurfaceNormalVelocityArcCondition : P1Condition
	{
		public VelocityOriginType VelocityType { get; set; }
		public CompareOperator Compare { get; set; }
		public float Arc { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<VelocityOriginType>(output, endianess, this.VelocityType);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Arc, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.VelocityType = BaseProperty.DeserializePropertyEnum<VelocityOriginType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Arc = input.ReadValueF32(endianess);
		}
	}
}
