using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(7730007216425001953UL)]
	public class MusicTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong GroupHash { get; set; }
		public ulong CueHash { get; set; }
		public ulong PartHash { get; set; }
		public MusicTrack.MusicPriority Priority { get; set; }
		public bool ResetPriority { get; set; }
		public bool OverrideFadeOut { get; set; }
		public int FadeoutStartBar { get; set; }
		public int FadeoutStartBeat { get; set; }
		public int FadeoutStartNote { get; set; }
		public int FadeoutLengthBar { get; set; }
		public int FadeoutLengthBeat { get; set; }
		public int FadeoutLengthNote { get; set; }
		public bool OverrideFadeIn { get; set; }
		public int FadeinStartBar { get; set; }
		public int FadeinStartBeat { get; set; }
		public int FadeinStartNote { get; set; }
		public int FadeinLengthBar { get; set; }
		public int FadeinLengthBeat { get; set; }
		public int FadeinLengthNote { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.GroupHash, endianess);
			output.WriteValueU64(this.CueHash, endianess);
			output.WriteValueU64(this.PartHash, endianess);
			BaseProperty.SerializePropertyEnum<MusicTrack.MusicPriority>(output, endianess, this.Priority);
			output.WriteValueB32(this.ResetPriority, endianess);
			output.WriteValueB32(this.OverrideFadeOut, endianess);
			output.WriteValueS32(this.FadeoutStartBar, endianess);
			output.WriteValueS32(this.FadeoutStartBeat, endianess);
			output.WriteValueS32(this.FadeoutStartNote, endianess);
			output.WriteValueS32(this.FadeoutLengthBar, endianess);
			output.WriteValueS32(this.FadeoutLengthBeat, endianess);
			output.WriteValueS32(this.FadeoutLengthNote, endianess);
			output.WriteValueB32(this.OverrideFadeIn, endianess);
			output.WriteValueS32(this.FadeinStartBar, endianess);
			output.WriteValueS32(this.FadeinStartBeat, endianess);
			output.WriteValueS32(this.FadeinStartNote, endianess);
			output.WriteValueS32(this.FadeinLengthBar, endianess);
			output.WriteValueS32(this.FadeinLengthBeat, endianess);
			output.WriteValueS32(this.FadeinLengthNote, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.GroupHash = input.ReadValueU64(endianess);
			this.CueHash = input.ReadValueU64(endianess);
			this.PartHash = input.ReadValueU64(endianess);
			this.Priority = BaseProperty.DeserializePropertyEnum<MusicTrack.MusicPriority>(input, endianess);
			this.ResetPriority = input.ReadValueB32(endianess);
			this.OverrideFadeOut = input.ReadValueB32(endianess);
			this.FadeoutStartBar = input.ReadValueS32(endianess);
			this.FadeoutStartBeat = input.ReadValueS32(endianess);
			this.FadeoutStartNote = input.ReadValueS32(endianess);
			this.FadeoutLengthBar = input.ReadValueS32(endianess);
			this.FadeoutLengthBeat = input.ReadValueS32(endianess);
			this.FadeoutLengthNote = input.ReadValueS32(endianess);
			this.OverrideFadeIn = input.ReadValueB32(endianess);
			this.FadeinStartBar = input.ReadValueS32(endianess);
			this.FadeinStartBeat = input.ReadValueS32(endianess);
			this.FadeinStartNote = input.ReadValueS32(endianess);
			this.FadeinLengthBar = input.ReadValueS32(endianess);
			this.FadeinLengthBeat = input.ReadValueS32(endianess);
			this.FadeinLengthNote = input.ReadValueS32(endianess);
		}
		public enum MusicPriority : ulong
		{
			Global = 12096938440000755089UL,
			Mission = 13148182856864167966UL
		}
	}
}
