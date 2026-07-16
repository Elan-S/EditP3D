using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.TokenSetFloatDelay)]
	public class TokenSetFloatDelayTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float FloatDelay { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.FloatDelay, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.FloatDelay = input.ReadValueF32(endianess);
		}
	}
}
