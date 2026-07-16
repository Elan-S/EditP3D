using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13378386982092559878UL)]
	public class ToneMapTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Enable { get; set; }
		public bool BlendFromPreviousState { get; set; }
		public float AdaptRate { get; set; }
		public float AverageLuminanceTarget { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Enable, endianess);
			output.WriteValueB32(this.BlendFromPreviousState, endianess);
			output.WriteValueF32(this.AdaptRate, endianess);
			output.WriteValueF32(this.AverageLuminanceTarget, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Enable = input.ReadValueB32(endianess);
			this.BlendFromPreviousState = input.ReadValueB32(endianess);
			this.AdaptRate = input.ReadValueF32(endianess);
			this.AverageLuminanceTarget = input.ReadValueF32(endianess);
		}
	}
}
