using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.ReactionHitSliceAngle)]
	public class ReactionHitSliceAngleCondition : P1Condition
	{
		public float Angle { get; set; }
		public float Arc { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueF32(this.Arc, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
		}
	}
}
