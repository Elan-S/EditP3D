using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Reverb)]
	public class ReverbTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Preset { get; set; }
		public float Gain { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Preset, endianess);
			output.WriteValueF32(this.Gain, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Preset = input.ReadValueU64(endianess);
			this.Gain = input.ReadValueF32(endianess);
		}
	}
}
