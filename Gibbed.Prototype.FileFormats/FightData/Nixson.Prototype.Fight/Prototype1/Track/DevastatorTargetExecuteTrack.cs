using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.DevastatorTargetExecute)]
	public class DevastatorTargetExecuteTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool UseAttackTimeBegin { get; set; }
		public bool UseAttackTimeEnd { get; set; }
		public bool SetTarget { get; set; }
		public BranchReference ReceiverBranch { get; set; } = new BranchReference();
		public int InterruptPriority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.UseAttackTimeBegin, endianess);
			output.WriteValueB32(this.UseAttackTimeEnd, endianess);
			output.WriteValueB32(this.SetTarget, endianess);
			this.ReceiverBranch.Serialize(output, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.UseAttackTimeBegin = input.ReadValueB32(endianess);
			this.UseAttackTimeEnd = input.ReadValueB32(endianess);
			this.SetTarget = input.ReadValueB32(endianess);
			this.ReceiverBranch = new BranchReference(input, endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
		}
	}
}
