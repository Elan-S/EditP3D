using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.AbortEffect)]
	public class AbortEffectTrack : P1Track
	{
		public ulong Slot { get; set; }
		public bool AllowFade { get; set; }
		public float FadeTime { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Slot, endianess);
			output.WriteValueB32(this.AllowFade, endianess);
			output.WriteValueF32(this.FadeTime, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Slot = input.ReadValueU64(endianess);
			this.AllowFade = input.ReadValueB32(endianess);
			this.FadeTime = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
