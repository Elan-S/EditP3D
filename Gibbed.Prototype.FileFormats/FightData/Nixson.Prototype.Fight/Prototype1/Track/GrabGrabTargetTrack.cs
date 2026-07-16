using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10762920317819628351UL)]
	public class GrabGrabTargetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong ParentJoint { get; set; }
		public Vector ParentPositionOffset { get; set; } = new Vector();
		public Vector ParentRotationOffset { get; set; } = new Vector();
		public ulong ChildJoint { get; set; }
		public Vector ChildPositionOffset { get; set; } = new Vector();
		public Vector ChildRotationOffset { get; set; } = new Vector();
		public float BlendTime { get; set; }
		public BranchReference GiverBranch { get; set; } = new BranchReference();
		public BranchReference ReceiverBranch { get; set; } = new BranchReference();
		public int InterruptPriority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.ParentJoint, endianess);
			this.ParentPositionOffset.Serialize(output, endianess);
			this.ParentRotationOffset.Serialize(output, endianess);
			output.WriteValueU64(this.ChildJoint, endianess);
			this.ChildPositionOffset.Serialize(output, endianess);
			this.ChildRotationOffset.Serialize(output, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
			this.GiverBranch.Serialize(output, endianess);
			this.ReceiverBranch.Serialize(output, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.ParentJoint = input.ReadValueU64(endianess);
			this.ParentPositionOffset = new Vector(input, endianess);
			this.ParentRotationOffset = new Vector(input, endianess);
			this.ChildJoint = input.ReadValueU64(endianess);
			this.ChildPositionOffset = new Vector(input, endianess);
			this.ChildRotationOffset = new Vector(input, endianess);
			this.BlendTime = input.ReadValueF32(endianess);
			this.GiverBranch = new BranchReference(input, endianess);
			this.ReceiverBranch = new BranchReference(input, endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
		}
	}
}
