using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11822946582678016064UL)]
	public class WOIPlayReactionTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong TargetGrabSlot { get; set; }
		public BranchReference WoiBranch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.TargetGrabSlot, endianess);
			this.WoiBranch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TargetGrabSlot = input.ReadValueU64(endianess);
			this.WoiBranch = new BranchReference(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
		}
	}
}
