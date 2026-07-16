using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(17114233817156247191UL)]
	public class ReactionHitHitTypeCondition : P1Condition
	{
		public bool DoesMatch { get; set; }
		public ulong HitType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.DoesMatch, endianess);
			output.WriteValueU64(this.HitType, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.DoesMatch = input.ReadValueB32(endianess);
			this.HitType = input.ReadValueU64(endianess);
		}
	}
}
