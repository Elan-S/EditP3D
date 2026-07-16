using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.OnlineTestForPlayerDeath)]
	public class OnlineTestForPlayerDeathCondition : P1Condition
	{
		public PlayerType Player { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.Player);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Player = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
		}
	}
}
