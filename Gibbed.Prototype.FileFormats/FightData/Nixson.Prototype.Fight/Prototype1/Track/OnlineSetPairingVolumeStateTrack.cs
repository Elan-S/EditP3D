using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(12819759230003973406UL)]
	public class OnlineSetPairingVolumeStateTrack : P1Track
	{
		public OnlineSetPairingVolumeStateTrack.OnlineVolumeType VolumeType { get; set; }
		public bool InVolume { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<OnlineSetPairingVolumeStateTrack.OnlineVolumeType>(output, endianess, this.VolumeType);
			output.WriteValueB32(this.InVolume, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.VolumeType = BaseProperty.DeserializePropertyEnum<OnlineSetPairingVolumeStateTrack.OnlineVolumeType>(input, endianess);
			this.InVolume = input.ReadValueB32(endianess);
		}
		public enum OnlineVolumeType : ulong
		{
			Inner = 5158950044111431452UL
		}
	}
}
