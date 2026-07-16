using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.HasCombatBudgetToken)]
	public class HasCombatBudgetTokenCondition : P1Condition
	{
		public bool RequestInAdvance { get; set; }
		public HasCombatBudgetTokenCondition.CombatEnumType CombatType { get; set; }
		public bool HasToken { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.RequestInAdvance, endianess);
			BaseProperty.SerializePropertyEnum<HasCombatBudgetTokenCondition.CombatEnumType>(output, endianess, this.CombatType);
			output.WriteValueB32(this.HasToken, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.RequestInAdvance = input.ReadValueB32(endianess);
			this.CombatType = BaseProperty.DeserializePropertyEnum<HasCombatBudgetTokenCondition.CombatEnumType>(input, endianess);
			this.HasToken = input.ReadValueB32(endianess);
		}
		public enum CombatEnumType : ulong
		{
			Bullet = 1071813355822642564UL,
			Rocket = 14395127206582377560UL,
			Explosion = 6119690315119812053UL,
			Gib = 305522356882UL
		}
	}
}
