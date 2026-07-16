using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Prototype1;
using Nixson.Prototype.Fight.Prototype2;

namespace Nixson.Prototype.Fight
{
	public abstract class BaseTrack : FightNode
	{
		public BaseTrack()
		{
			KnownTrackAttribute knownTrackAttribute = (KnownTrackAttribute)base.GetType().GetCustomAttributes(typeof(KnownTrackAttribute), false)[0];
			base.TypeHash = knownTrackAttribute.Hash;
		}
		public override object Clone(PrototypeGame game)
		{
			Stream stream = new MemoryStream();
			BaseTrack.SerializeBaseTrack(game, stream, Endian.Little, this);
			stream.Position = 0L;
			ulong hash = stream.ReadValueU64();
			return BaseTrack.DeserializeBaseTrack(game, stream, Endian.Little, hash);
		}
		public static void SerializeBaseTrack(PrototypeGame game, Stream output, Endian endianess, BaseTrack track)
		{
			if (game == PrototypeGame.P1)
			{
				P1Track.SerializeBaseTrack(output, endianess, track);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			P2Track.SerializeBaseTrack(output, endianess, track);
		}
		public static void SerializeBaseTracks(PrototypeGame game, Stream output, Endian endianess, List<BaseTrack> tracks)
		{
			if (game == PrototypeGame.P1)
			{
				P1Track.SerializeBaseTracks(output, endianess, tracks);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			P2Track.SerializeBaseTracks(output, endianess, tracks);
		}
		public static BaseTrack DeserializeBaseTrack(PrototypeGame game, Stream input, Endian endianess, ulong hash)
		{
			if (game == PrototypeGame.P1)
			{
				return P1Track.DeserializeBaseTrack(input, endianess, hash);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return P2Track.DeserializeBaseTrack(input, endianess, hash);
		}
		public static List<BaseTrack> DeserializeBaseTracks(PrototypeGame game, Stream input, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				return P1Track.DeserializeBaseTracks(input, endianess);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return P2Track.DeserializeBaseTracks(input, endianess);
		}
	}
}
