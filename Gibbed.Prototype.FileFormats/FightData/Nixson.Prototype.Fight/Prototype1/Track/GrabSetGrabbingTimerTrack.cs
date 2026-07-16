using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.GrabSetGrabbingTimer)]
	public class GrabSetGrabbingTimerTrack : P1Track
	{
		public float Duration { get; set; }
		public bool IsGrabAttack { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.Duration, endianess);
			output.WriteValueB32(this.IsGrabAttack, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Duration = input.ReadValueF32(endianess);
			this.IsGrabAttack = input.ReadValueB32(endianess);
		}
	}
}
