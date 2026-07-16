using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14479430701764144130UL)]
	public class OnlineFlagPlayerDeathTrack : P1Track
	{
		public PlayerType PlayerToTell { get; set; }
		public PlayerType PlayerThatDied { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.PlayerToTell);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.PlayerThatDied);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.PlayerToTell = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
			this.PlayerThatDied = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
		}
	}
}
