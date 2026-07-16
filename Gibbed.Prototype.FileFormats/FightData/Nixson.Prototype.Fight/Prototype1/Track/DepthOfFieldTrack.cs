using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10541480185248453832UL)]
	public class DepthOfFieldTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Enable { get; set; }
		public bool BlendFromPreviousState { get; set; }
		public float NearDepth { get; set; }
		public float FarDepth { get; set; }
		public float Range { get; set; }
		public float Aperture { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Enable, endianess);
			output.WriteValueB32(this.BlendFromPreviousState, endianess);
			output.WriteValueF32(this.NearDepth, endianess);
			output.WriteValueF32(this.FarDepth, endianess);
			output.WriteValueF32(this.Range, endianess);
			output.WriteValueF32(this.Aperture, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Enable = input.ReadValueB32(endianess);
			this.BlendFromPreviousState = input.ReadValueB32(endianess);
			this.NearDepth = input.ReadValueF32(endianess);
			this.FarDepth = input.ReadValueF32(endianess);
			this.Range = input.ReadValueF32(endianess);
			this.Aperture = input.ReadValueF32(endianess);
		}
	}
}
