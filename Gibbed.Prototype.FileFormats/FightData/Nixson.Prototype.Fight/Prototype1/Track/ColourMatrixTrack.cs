using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ColourMatrix)]
	public class ColourMatrixTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Enable { get; set; }
		public bool BlendFromPreviousState { get; set; }
		public float Brightness { get; set; }
		public float Contrast { get; set; }
		public float Midpoint { get; set; }
		public float Saturate { get; set; }
		public float Hue { get; set; }
		public Color Tint { get; set; } = new Color();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Enable, endianess);
			output.WriteValueB32(this.BlendFromPreviousState, endianess);
			output.WriteValueF32(this.Brightness, endianess);
			output.WriteValueF32(this.Contrast, endianess);
			output.WriteValueF32(this.Midpoint, endianess);
			output.WriteValueF32(this.Saturate, endianess);
			output.WriteValueF32(this.Hue, endianess);
			this.Tint.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Enable = input.ReadValueB32(endianess);
			this.BlendFromPreviousState = input.ReadValueB32(endianess);
			this.Brightness = input.ReadValueF32(endianess);
			this.Contrast = input.ReadValueF32(endianess);
			this.Midpoint = input.ReadValueF32(endianess);
			this.Saturate = input.ReadValueF32(endianess);
			this.Hue = input.ReadValueF32(endianess);
			this.Tint.Deserialize(input, endianess);
		}
	}
}
