using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11693099025353764666UL)]
	public class JointLookAtCondition : P1Condition
	{
		public ulong Joint { get; set; }
		public Vector Heading { get; set; } = new Vector();
		public Vector PrimaryRotationAxis { get; set; } = new Vector();
		public float PrimaryAngleMin { get; set; }
		public float PrimaryAngleMax { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Heading.Serialize(output, endianess);
			this.PrimaryRotationAxis.Serialize(output, endianess);
			output.WriteValueF32(this.PrimaryAngleMin, endianess);
			output.WriteValueF32(this.PrimaryAngleMax, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Heading.Deserialize(input, endianess);
			this.PrimaryRotationAxis.Deserialize(input, endianess);
			this.PrimaryAngleMin = input.ReadValueF32(endianess);
			this.PrimaryAngleMax = input.ReadValueF32(endianess);
		}
	}
}
