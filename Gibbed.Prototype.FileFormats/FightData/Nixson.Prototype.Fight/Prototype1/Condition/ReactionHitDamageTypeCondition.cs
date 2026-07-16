using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(17045940696370927199UL)]
	public class ReactionHitDamageTypeCondition : P1Condition
	{
		public bool DoesMatch { get; set; }
		public DamageType DamageType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.DoesMatch, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.DoesMatch = input.ReadValueB32(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
		}
	}
}
