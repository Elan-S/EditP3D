using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Scrubber)]
	public class ScrubberTrack : P1Track
	{
		public ulong Animation { get; set; }
		public float Frame { get; set; }
		public float FrameOffset { get; set; }
		public bool HasRootTranslation { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Animation, endianess);
			output.WriteValueF32(this.Frame, endianess);
			output.WriteValueF32(this.FrameOffset, endianess);
			output.WriteValueB32(this.HasRootTranslation, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Animation = input.ReadValueU64(endianess);
			this.Frame = input.ReadValueF32(endianess);
			this.FrameOffset = input.ReadValueF32(endianess);
			this.HasRootTranslation = input.ReadValueB32(endianess);
		}
	}
}
