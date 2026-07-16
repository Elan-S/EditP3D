using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(16838968172027538993UL)]
	public class ReactionHitCollisionNormalCondition : P1Condition
	{
		public Vector Normal { get; set; } = new Vector();
		public float Arc { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			this.Normal.Serialize(output, endianess);
			output.WriteValueF32(this.Arc, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Normal.Deserialize(input, endianess);
			this.Arc = input.ReadValueF32(endianess);
		}
	}
}
