using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.OnlineTestForRespawnState)]
	public class OnlineTestForRespawnStateCondition : P1Condition
	{
		public PlayerType Player { get; set; }
		public OnlineStateType State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.Player);
			BaseProperty.SerializePropertyEnum<OnlineStateType>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Player = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
			this.State = BaseProperty.DeserializePropertyEnum<OnlineStateType>(input, endianess);
		}
	}
}
