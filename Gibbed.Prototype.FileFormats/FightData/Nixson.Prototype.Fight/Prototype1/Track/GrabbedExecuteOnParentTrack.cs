using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(16769375240862877143UL)]
	public class GrabbedExecuteOnParentTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public BranchReference ReceiverBranch { get; set; } = new BranchReference();
		public int InterruptPriority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			this.ReceiverBranch.Serialize(output, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ReceiverBranch = new BranchReference(input, endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
		}
	}
}
