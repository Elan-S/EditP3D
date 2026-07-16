using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(18231527313455387908UL)]
	public class GrabbedPlaybackTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong State { get; set; }
		public ulong SpecificPlaybackSet { get; set; }
		public bool NotifyOnPlaybackFinished { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.State, endianess);
			output.WriteValueU64(this.SpecificPlaybackSet, endianess);
			output.WriteValueB32(this.NotifyOnPlaybackFinished, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.State = input.ReadValueU64(endianess);
			this.SpecificPlaybackSet = input.ReadValueU64(endianess);
			this.NotifyOnPlaybackFinished = input.ReadValueB32(endianess);
		}
	}
}
