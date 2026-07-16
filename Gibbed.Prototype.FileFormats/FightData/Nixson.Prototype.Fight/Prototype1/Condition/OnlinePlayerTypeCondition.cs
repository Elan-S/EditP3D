using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.OnlinePlayerType)]
	public class OnlinePlayerTypeCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public PlayerType Player { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.Player);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Player = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
		}
	}
}
