using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11147314434280893799UL)]
	public class ReactionHitDamageCondition : P1Condition
	{
		public ReactionHitDamageCondition.ReationDamageType Type { get; set; }
		public CompareOperator Compare { get; set; }
		public float Damage { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ReactionHitDamageCondition.ReationDamageType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Damage, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<ReactionHitDamageCondition.ReationDamageType>(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Damage = input.ReadValueF32(endianess);
		}
		public enum ReationDamageType : ulong
		{
			DamageGiven = 8477540622776547364UL,
			DamageTaken = 7591924666722235046UL
		}
	}
}
