using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Bloom)]
	public class BloomTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool Enable { get; set; }
		public bool DisableOnFinish { get; set; }
		public bool BlendFromPreviousState { get; set; }
		public float Brightness { get; set; }
		public float Contrast { get; set; }
		public float Midpoint { get; set; }
		public float Saturate { get; set; }
		public float Hue { get; set; }
		public Color Tint { get; set; } = new Color();
		public float Bloom { get; set; }
		public float Backbuffer { get; set; }
		public float RadiusX { get; set; }
		public float RadiusY { get; set; }
		public Color Colour { get; set; } = new Color();
		public float LuminanceCutoff { get; set; }
		public float Falloff { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.Enable, endianess);
			output.WriteValueB32(this.DisableOnFinish, endianess);
			output.WriteValueB32(this.BlendFromPreviousState, endianess);
			output.WriteValueF32(this.Brightness, endianess);
			output.WriteValueF32(this.Contrast, endianess);
			output.WriteValueF32(this.Midpoint, endianess);
			output.WriteValueF32(this.Saturate, endianess);
			output.WriteValueF32(this.Hue, endianess);
			this.Tint.Serialize(output, endianess);
			output.WriteValueF32(this.Bloom, endianess);
			output.WriteValueF32(this.Backbuffer, endianess);
			output.WriteValueF32(this.RadiusX, endianess);
			output.WriteValueF32(this.RadiusY, endianess);
			this.Colour.Serialize(output, endianess);
			output.WriteValueF32(this.LuminanceCutoff, endianess);
			output.WriteValueF32(this.Falloff, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Enable = input.ReadValueB32(endianess);
			this.DisableOnFinish = input.ReadValueB32(endianess);
			this.BlendFromPreviousState = input.ReadValueB32(endianess);
			this.Brightness = input.ReadValueF32(endianess);
			this.Contrast = input.ReadValueF32(endianess);
			this.Midpoint = input.ReadValueF32(endianess);
			this.Saturate = input.ReadValueF32(endianess);
			this.Hue = input.ReadValueF32(endianess);
			this.Tint.Deserialize(input, endianess);
			this.Bloom = input.ReadValueF32(endianess);
			this.Backbuffer = input.ReadValueF32(endianess);
			this.RadiusX = input.ReadValueF32(endianess);
			this.RadiusY = input.ReadValueF32(endianess);
			this.Colour.Deserialize(input, endianess);
			this.LuminanceCutoff = input.ReadValueF32(endianess);
			this.Falloff = input.ReadValueF32(endianess);
		}
	}
}
