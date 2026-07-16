using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.HaveToken)]
	public class HaveTokenCondition : P1Condition
	{
		public HaveTokenCondition.SomethingTokenType Type { get; set; }
		public HaveTokenCondition.OwnerType Owner { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<HaveTokenCondition.SomethingTokenType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyEnum<HaveTokenCondition.OwnerType>(output, endianess, this.Owner);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<HaveTokenCondition.SomethingTokenType>(input, endianess);
			this.Owner = BaseProperty.DeserializePropertyEnum<HaveTokenCondition.OwnerType>(input, endianess);
		}
		public enum SomethingTokenType : ulong
		{
			hunter = 6865395180375055270UL,
			fireClearRequest = 7926183158331532086UL
		}
		public enum OwnerType : ulong
		{
			Me = 7150262UL,
			Anyone = 11551537155431939438UL
		}
	}
}
