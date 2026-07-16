using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Alert)]
	[KnownTrack(TrackHash.ReportBackToIdle)]
	public class ReportBackToIdleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Evade { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Evade, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Evade = input.ReadValueB32(endianess);
		}
	}
}
