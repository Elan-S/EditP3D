using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight.Prototype1
{
	public abstract class P1Track : BaseTrack
	{
		public uint Unknown1 { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			output.WriteValueU32(this.Unknown1, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			this.Unknown1 = input.ReadValueU32(endianess);
		}
		public static void SerializeBaseTrack(Stream output, Endian endianess, BaseTrack track)
		{
			Stream stream = new MemoryStream();
			track.Serialize(stream, endianess);
			output.WriteValueU64(track.TypeHash, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			track.SerializeProperties(PrototypeGame.P1, output, endianess);
			output.WriteValueU64(0UL);
		}
		public static void SerializeBaseTracks(Stream output, Endian endianess, List<BaseTrack> tracks)
		{
			foreach (BaseTrack track in tracks)
			{
				P1Track.SerializeBaseTrack(output, endianess, track);
			}
		}
		public static BaseTrack DeserializeBaseTrack(Stream input, Endian endianess, ulong hash)
		{
			BaseTrack baseTrack = Factory<BaseTrack, KnownTrackAttribute>.Build(PrototypeGame.P1, hash);
			if (baseTrack == null)
			{
				throw new NotImplementedException("Unknown track");
			}
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			baseTrack.Deserialize(input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid track length");
			}
			baseTrack.DeserializeProperties(PrototypeGame.P1, input, endianess);
			input.ReadValueU64();
			return baseTrack;
		}
		public static List<BaseTrack> DeserializeBaseTracks(Stream input, Endian endianess)
		{
			List<BaseTrack> list = new List<BaseTrack>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				list.Add(P1Track.DeserializeBaseTrack(input, endianess, num));
			}
			input.Position -= 8L;
			return list;
		}
	}
}
