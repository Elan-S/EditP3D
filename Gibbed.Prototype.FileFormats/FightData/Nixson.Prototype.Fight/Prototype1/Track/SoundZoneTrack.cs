using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17218410905236931483UL)]
	public class SoundZoneTrack : P1Track
	{
		public ulong ZoneName { get; set; }
		public ulong Reverb { get; set; }
		public ulong Ambience { get; set; }
		public ulong Mix { get; set; }
		public SoundZoneTrack.BoundaryActionType BoundaryAction { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.ZoneName, endianess);
			output.WriteValueU64(this.Reverb, endianess);
			output.WriteValueU64(this.Ambience, endianess);
			output.WriteValueU64(this.Mix, endianess);
			BaseProperty.SerializePropertyEnum<SoundZoneTrack.BoundaryActionType>(output, endianess, this.BoundaryAction);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ZoneName = input.ReadValueU64(endianess);
			this.Reverb = input.ReadValueU64(endianess);
			this.Ambience = input.ReadValueU64(endianess);
			this.Mix = input.ReadValueU64(endianess);
			this.BoundaryAction = BaseProperty.DeserializePropertyEnum<SoundZoneTrack.BoundaryActionType>(input, endianess);
		}
		public enum BoundaryActionType : ulong
		{
			ZoneEnter = 3205891085038241972UL,
			ZoneExit = 12785862139907654626UL
		}
	}
}
