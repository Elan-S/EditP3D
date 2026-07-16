using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10076790508117696542UL)]
	public class ThreatSendToTargetTrack : P1Track
	{
		public ulong ThreatName { get; set; }
		public ulong ThrownObjectFromGrabSlot { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.ThreatName, endianess);
			output.WriteValueU64(this.ThrownObjectFromGrabSlot, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ThreatName = input.ReadValueU64(endianess);
			this.ThrownObjectFromGrabSlot = input.ReadValueU64(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
