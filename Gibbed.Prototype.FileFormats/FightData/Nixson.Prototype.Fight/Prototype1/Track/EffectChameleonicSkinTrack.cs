using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17485702295865601948UL)]
	public class EffectChameleonicSkinTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public EffectChameleonicSkinTrack.ActionEventType ActionOnBegin { get; set; }
		public EffectChameleonicSkinTrack.ActionEventType ActionOnEnd { get; set; }
		public float WarpingFactor { get; set; }
		public float WarpOffsetScale { get; set; }
		public EffectChameleonicSkinTrack.ColourEnumType ColourType { get; set; }
		public Color Colour { get; set; } = new Color();
		public float ColourMix { get; set; }
		public float BlurRadius { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<EffectChameleonicSkinTrack.ActionEventType>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<EffectChameleonicSkinTrack.ActionEventType>(output, endianess, this.ActionOnEnd);
			output.WriteValueF32(this.WarpingFactor, endianess);
			output.WriteValueF32(this.WarpOffsetScale, endianess);
			BaseProperty.SerializePropertyEnum<EffectChameleonicSkinTrack.ColourEnumType>(output, endianess, this.ColourType);
			this.Colour.Serialize(output, endianess);
			output.WriteValueF32(this.ColourMix, endianess);
			output.WriteValueF32(this.BlurRadius, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<EffectChameleonicSkinTrack.ActionEventType>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<EffectChameleonicSkinTrack.ActionEventType>(input, endianess);
			this.WarpingFactor = input.ReadValueF32(endianess);
			this.WarpOffsetScale = input.ReadValueF32(endianess);
			this.ColourType = BaseProperty.DeserializePropertyEnum<EffectChameleonicSkinTrack.ColourEnumType>(input, endianess);
			this.Colour.Deserialize(input, endianess);
			this.ColourMix = input.ReadValueF32(endianess);
			this.BlurRadius = input.ReadValueF32(endianess);
		}
		public enum ActionEventType : ulong
		{
			Enable = 8038335290554022053UL,
			Disable = 9965250516041879004UL,
			None = 22018610510307286UL
		}
		public enum ColourEnumType : ulong
		{
			Prototype = 11199460887736065746UL,
			Infected = 13743031593724774280UL
		}
	}
}
