using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(TrackHash.Mixer)]
	public class MixerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float FadeTime { get; set; }
		public ulong MixerHash { get; set; }
		public ulong CategoryHash { get; set; }
		public bool FadeIn { get; set; }
		public bool UninstallOnExit { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.FadeTime, endianess);
			output.WriteValueU64(this.MixerHash, endianess);
			output.WriteValueU64(this.CategoryHash, endianess);
			output.WriteValueB32(this.FadeIn, endianess);
			output.WriteValueB32(this.UninstallOnExit, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.FadeTime = input.ReadValueF32(endianess);
			this.MixerHash = input.ReadValueU64(endianess);
			this.CategoryHash = input.ReadValueU64(endianess);
			this.FadeIn = input.ReadValueB32(endianess);
			this.UninstallOnExit = input.ReadValueB32(endianess);
		}
	}
}
