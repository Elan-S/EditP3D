using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(TrackHash.FMV)]
	public class FMVTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public string Filename { get; set; }
		public bool UseFMVState { get; set; }
		public bool ClearSubtitles { get; set; }
		public bool IsPreLoaded { get; set; }
		public bool UseBlackFades { get; set; }
		public bool FadeInOnEnter { get; set; }
		public bool StayFadedOnExit { get; set; }
		public bool FadeUpOnExit { get; set; }
		public bool DelayUninstall { get; set; }
		public float UninstallDelayTime { get; set; }
		public float WhiteFadeTime { get; set; }
		public float Timer { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteStringAlignedU32(this.Filename, endianess);
			output.WriteValueB32(this.UseFMVState, endianess);
			output.WriteValueB32(this.ClearSubtitles, endianess);
			output.WriteValueB32(this.IsPreLoaded, endianess);
			output.WriteValueB32(this.UseBlackFades, endianess);
			output.WriteValueB32(this.FadeInOnEnter, endianess);
			output.WriteValueB32(this.StayFadedOnExit, endianess);
			output.WriteValueB32(this.FadeUpOnExit, endianess);
			output.WriteValueB32(this.DelayUninstall, endianess);
			output.WriteValueF32(this.UninstallDelayTime, endianess);
			output.WriteValueF32(this.WhiteFadeTime, endianess);
			output.WriteValueF32(this.Timer, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Filename = input.ReadStringAlignedU32(endianess);
			this.UseFMVState = input.ReadValueB32(endianess);
			this.ClearSubtitles = input.ReadValueB32(endianess);
			this.IsPreLoaded = input.ReadValueB32(endianess);
			this.UseBlackFades = input.ReadValueB32(endianess);
			this.FadeInOnEnter = input.ReadValueB32(endianess);
			this.StayFadedOnExit = input.ReadValueB32(endianess);
			this.FadeUpOnExit = input.ReadValueB32(endianess);
			this.DelayUninstall = input.ReadValueB32(endianess);
			this.UninstallDelayTime = input.ReadValueF32(endianess);
			this.WhiteFadeTime = input.ReadValueF32(endianess);
			this.Timer = input.ReadValueF32(endianess);
		}
	}
}
