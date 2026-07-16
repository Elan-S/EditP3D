using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(9515996776968559110UL)]
	public class SupportingLimbTestSurfaceVelocityAngleCondition : P1Condition
	{
		public VelocityOriginType VelocityType { get; set; }
		public Vector ZeroVector { get; set; } = new Vector();
		public float Angle { get; set; }
		public float Arc { get; set; }
		public bool UseExcessVelocity { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<VelocityOriginType>(output, endianess, this.VelocityType);
			this.ZeroVector.Serialize(output, endianess);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueB32(this.UseExcessVelocity, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.VelocityType = BaseProperty.DeserializePropertyEnum<VelocityOriginType>(input, endianess);
			this.ZeroVector = new Vector(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.UseExcessVelocity = input.ReadValueB32(endianess);
		}
	}
}
