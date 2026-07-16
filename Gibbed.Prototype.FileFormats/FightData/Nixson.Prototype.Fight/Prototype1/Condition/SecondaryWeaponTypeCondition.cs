using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.SecondaryWeaponType)]
	public class SecondaryWeaponTypeCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public SecondaryWeaponTypeCondition.SecondaryWeaponType Type { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			BaseProperty.SerializePropertyEnum<SecondaryWeaponTypeCondition.SecondaryWeaponType>(output, endianess, this.Type);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<SecondaryWeaponTypeCondition.SecondaryWeaponType>(input, endianess);
		}
		public enum SecondaryWeaponType : ulong
		{
			Gun50mm = 13590193798748521041UL,
			Rocket = 14395127206582377560UL,
			Missile = 13148182856864364872UL,
			Bloodtox = 18124356316935178433UL
		}
	}
}
