using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight.Property
{
	public class PropertyTrackGroup : BaseProperty
	{
		public List<BaseTrack> Tracks { get; set; } = new List<BaseTrack>();
		public PropertyTrackGroup()
		{
		}
		public PropertyTrackGroup(PropertyHash hash) : base(hash)
		{
		}
		public override object Clone(PrototypeGame game)
		{
			Stream stream = new MemoryStream();
			BaseProperty.SerializeBaseProperty(game, stream, Endian.Little, this);
			stream.Position = 0L;
			ulong hash = stream.ReadValueU64();
			stream.Position = 0L;
			return BaseProperty.DeserializeTrackProperty(game, stream, Endian.Little, (PropertyHash)hash);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				this.P1_SerializeProperties(output, endianess);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			this.P2_SerializeProperties(output, endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				this.P1_DeserializeProperties(input, endianess);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			this.P2_DeserializeProperties(input, endianess);
		}
		private void P1_SerializeProperties(Stream output, Endian endianess)
		{
			BaseTrack.SerializeBaseTracks(PrototypeGame.P1, output, endianess, this.Tracks);
		}
		private void P1_DeserializeProperties(Stream input, Endian endianess)
		{
			this.Tracks = new List<BaseTrack>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				BaseTrack item = BaseTrack.DeserializeBaseTrack(PrototypeGame.P1, input, endianess, num);
				this.Tracks.Add(item);
			}
		}
		private void P2_SerializeProperties(Stream output, Endian endianess)
		{
			BaseTrack.SerializeBaseTracks(PrototypeGame.P2, output, endianess, this.Tracks);
		}
		private void P2_DeserializeProperties(Stream input, Endian endianess)
		{
			this.Tracks = new List<BaseTrack>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				BaseTrack item = BaseTrack.DeserializeBaseTrack(PrototypeGame.P2, input, endianess, num);
				this.Tracks.Add(item);
			}
			input.Position -= 8L;
		}
	}
}
