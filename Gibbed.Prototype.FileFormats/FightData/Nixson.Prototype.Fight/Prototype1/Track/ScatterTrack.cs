using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(16094547547874119018UL)]
	public class ScatterTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Enable { get; set; }
		public bool BlendFromPreviousState { get; set; }
		public Color ScatterColour { get; set; } = new Color();
		public float SaturationMin { get; set; }
		public float SaturationMax { get; set; }
		public float ContrastMin { get; set; }
		public float ContrastMax { get; set; }
		public float BrightnessMin { get; set; }
		public float BrightnessMax { get; set; }
		public float TintMin { get; set; }
		public float TintMax { get; set; }
		public float DistanceBrightening { get; set; }
		public float ContrastDropoff { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Enable, endianess);
			output.WriteValueB32(this.BlendFromPreviousState, endianess);
			this.ScatterColour.Serialize(output, endianess);
			output.WriteValueF32(this.SaturationMin, endianess);
			output.WriteValueF32(this.SaturationMax, endianess);
			output.WriteValueF32(this.ContrastMin, endianess);
			output.WriteValueF32(this.ContrastMax, endianess);
			output.WriteValueF32(this.BrightnessMin, endianess);
			output.WriteValueF32(this.BrightnessMax, endianess);
			output.WriteValueF32(this.TintMin, endianess);
			output.WriteValueF32(this.TintMax, endianess);
			output.WriteValueF32(this.DistanceBrightening, endianess);
			output.WriteValueF32(this.ContrastDropoff, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Enable = input.ReadValueB32(endianess);
			this.BlendFromPreviousState = input.ReadValueB32(endianess);
			this.ScatterColour.Deserialize(input, endianess);
			this.SaturationMin = input.ReadValueF32(endianess);
			this.SaturationMax = input.ReadValueF32(endianess);
			this.ContrastMin = input.ReadValueF32(endianess);
			this.ContrastMax = input.ReadValueF32(endianess);
			this.BrightnessMin = input.ReadValueF32(endianess);
			this.BrightnessMax = input.ReadValueF32(endianess);
			this.TintMin = input.ReadValueF32(endianess);
			this.TintMax = input.ReadValueF32(endianess);
			this.DistanceBrightening = input.ReadValueF32(endianess);
			this.ContrastDropoff = input.ReadValueF32(endianess);
		}
	}
}
