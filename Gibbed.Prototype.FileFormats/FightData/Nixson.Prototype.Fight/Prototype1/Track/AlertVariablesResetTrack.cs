using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Alert)]
	[KnownTrack(TrackHash.AlertVariablesReset)]
	public class AlertVariablesResetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool ResetJustTime { get; set; }
		public bool DontResetFactionIfSniffer { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.ResetJustTime, endianess);
			output.WriteValueB32(this.DontResetFactionIfSniffer, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ResetJustTime = input.ReadValueB32(endianess);
			this.DontResetFactionIfSniffer = input.ReadValueB32(endianess);
		}
	}
}
