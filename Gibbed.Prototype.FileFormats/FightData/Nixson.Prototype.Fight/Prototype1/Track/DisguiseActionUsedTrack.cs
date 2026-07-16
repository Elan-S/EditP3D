using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.DisguiseActionUsed)]
	public class DisguiseActionUsedTrack : P1Track
	{
		public DisguiseActionUsedTrack.DisguiseActionType Action { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<DisguiseActionUsedTrack.DisguiseActionType>(output, endianess, this.Action);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Action = BaseProperty.DeserializePropertyEnum<DisguiseActionUsedTrack.DisguiseActionType>(input, endianess);
		}
		public enum DisguiseActionType : ulong
		{
			Patsy = 5692038329941528275UL,
			AirStrike = 10337102137488492662UL,
			CallStrikeTeam = 3853207631728303317UL
		}
	}
}
