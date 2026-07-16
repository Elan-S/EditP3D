using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.FactionSet)]
	public class FactionSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Faction { get; set; }
		public bool NotifyAI { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Faction, endianess);
			output.WriteValueB32(this.NotifyAI, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Faction = input.ReadValueU64(endianess);
			this.NotifyAI = input.ReadValueB32(endianess);
		}
	}
}
