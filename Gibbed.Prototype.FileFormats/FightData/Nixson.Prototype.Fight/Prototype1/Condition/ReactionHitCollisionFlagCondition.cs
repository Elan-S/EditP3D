using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(17959467439043560562UL)]
	public class ReactionHitCollisionFlagCondition : P1Condition
	{
		public ulong CollisionFlag { get; set; }
		public bool DoesMatch { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.CollisionFlag, endianess);
			output.WriteValueB32(this.DoesMatch, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.CollisionFlag = input.ReadValueU64(endianess);
			this.DoesMatch = input.ReadValueB32(endianess);
		}
	}
}
