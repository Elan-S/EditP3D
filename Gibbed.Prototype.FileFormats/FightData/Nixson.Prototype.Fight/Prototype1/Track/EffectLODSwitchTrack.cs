using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14793002886934743980UL)]
	public class EffectLODSwitchTrack : P1Track
	{
		public float LodStartDistance { get; set; }
		public float LodUpdateRateMilliseconds { get; set; }
		public PropertyTrackGroup Effect { get; set; } = new PropertyTrackGroup(PropertyHash.Effect);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.LodStartDistance, endianess);
			output.WriteValueF32(this.LodUpdateRateMilliseconds, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.LodStartDistance = input.ReadValueF32(endianess);
			this.LodUpdateRateMilliseconds = input.ReadValueF32(endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Effect);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Effect = BaseProperty.DeserializeTrackProperty(PrototypeGame.P1, input, endianess, PropertyHash.Effect);
		}
	}
}
