using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11083546118944390151UL)]
	public class HideHUDTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public int HideIndividualElements { get; set; }
		public HideHUDTrack.HUDElement HideIndividualElementsHideElements { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueS32(this.HideIndividualElements, endianess);
			BaseProperty.SerializePropertyBitfield<HideHUDTrack.HUDElement>(output, endianess, this.HideIndividualElementsHideElements);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.HideIndividualElements = input.ReadValueS32(endianess);
			this.HideIndividualElementsHideElements = BaseProperty.DeserializePropertyBitfield<HideHUDTrack.HUDElement>(input, endianess);
		}
		[Flags]
		public enum HUDElement : ulong
		{
			MINIMAP = 1UL,
			HEALTH = 2UL,
			POWERS = 4UL,
			POPUP = 8UL,
			DISGUISE_PERCEPTION = 16UL,
			BUTTON_HINT = 32UL,
			TARGETING = 64UL,
			EVOLUTION_POINTS = 128UL,
			METER = 256UL,
			TEXTLOG = 512UL,
			VEHICLE_HEALTH = 1024UL,
			MARKERS = 2048UL,
			KEYCARD = 4096UL,
			DISGUISE_ACTIONS = 8192UL,
			AWARENESS = 16384UL,
			CCR = 32768UL
		}
	}
}
