using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Light)]
	public class LightTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong LightName { get; set; }
		public int Colour { get; set; }
		public Color ColourColour { get; set; } = new Color();
		public int Intensity { get; set; }
		public float IntensityIntensity { get; set; }
		public int ShadowCaster { get; set; }
		public float ShadowCasterShadowOffset { get; set; }
		public float ShadowCasterRangeMultiplier { get; set; }
		public float ShadowCasterRadiusMultiplier { get; set; }
		public float ShadowCasterMaxRadius { get; set; }
		public int Umbra { get; set; }
		public float UmbraUmbra { get; set; }
		public float UmbraPenumbra { get; set; }
		public int Decay { get; set; }
		public float DecayInner { get; set; }
		public float DecayOuter { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.LightName, endianess);
			output.WriteValueS32(this.Colour, endianess);
			this.ColourColour.Serialize(output, endianess);
			output.WriteValueS32(this.Intensity, endianess);
			output.WriteValueF32(this.IntensityIntensity, endianess);
			output.WriteValueS32(this.ShadowCaster, endianess);
			output.WriteValueF32(this.ShadowCasterShadowOffset, endianess);
			output.WriteValueF32(this.ShadowCasterRangeMultiplier, endianess);
			output.WriteValueF32(this.ShadowCasterRadiusMultiplier, endianess);
			output.WriteValueF32(this.ShadowCasterMaxRadius, endianess);
			output.WriteValueS32(this.Umbra, endianess);
			output.WriteValueF32(this.UmbraUmbra, endianess);
			output.WriteValueF32(this.UmbraPenumbra, endianess);
			output.WriteValueS32(this.Decay, endianess);
			output.WriteValueF32(this.DecayInner, endianess);
			output.WriteValueF32(this.DecayOuter, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.LightName = input.ReadValueU64(endianess);
			this.Colour = input.ReadValueS32(endianess);
			this.ColourColour.Deserialize(input, endianess);
			this.Intensity = input.ReadValueS32(endianess);
			this.IntensityIntensity = input.ReadValueF32(endianess);
			this.ShadowCaster = input.ReadValueS32(endianess);
			this.ShadowCasterShadowOffset = input.ReadValueF32(endianess);
			this.ShadowCasterRangeMultiplier = input.ReadValueF32(endianess);
			this.ShadowCasterRadiusMultiplier = input.ReadValueF32(endianess);
			this.ShadowCasterMaxRadius = input.ReadValueF32(endianess);
			this.Umbra = input.ReadValueS32(endianess);
			this.UmbraUmbra = input.ReadValueF32(endianess);
			this.UmbraPenumbra = input.ReadValueF32(endianess);
			this.Decay = input.ReadValueS32(endianess);
			this.DecayInner = input.ReadValueF32(endianess);
			this.DecayOuter = input.ReadValueF32(endianess);
		}
	}
}
