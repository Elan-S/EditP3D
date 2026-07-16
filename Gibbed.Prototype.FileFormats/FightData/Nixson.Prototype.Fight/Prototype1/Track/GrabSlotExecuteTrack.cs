using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.GrabSlotExecute)]
	public class GrabSlotExecuteTrack : P1Track
	{
		public float BeginTime { get; set; }
		public ulong GrabSlotName { get; set; }
		public BranchReference ReceiverBranchRef { get; set; } = new BranchReference();
		public int InterruptPriority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueU64(this.GrabSlotName, endianess);
			this.ReceiverBranchRef.Serialize(output, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.GrabSlotName = input.ReadValueU64(endianess);
			this.ReceiverBranchRef = new BranchReference(input, endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
		}
	}
}
