using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14189974345323155369UL)]
	public class ConsumeByParentTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong RagdollIfNeeded { get; set; }
		public bool SwitchToRagdollMode { get; set; }
		public ulong ParentObjectName { get; set; }
		public ulong ParentDestinationJoint { get; set; }
		public Vector ParentPositionOffset { get; set; } = new Vector();
		public bool ConsumeUpwards { get; set; }
		public float BoneSweepConstant { get; set; }
		public bool TransformationConsume { get; set; }
		public float TransformationConsumeTime { get; set; }
		public bool Stealthy { get; set; }
		public ulong ConsumableProperties { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.RagdollIfNeeded, endianess);
			output.WriteValueB32(this.SwitchToRagdollMode, endianess);
			output.WriteValueU64(this.ParentObjectName, endianess);
			output.WriteValueU64(this.ParentDestinationJoint, endianess);
			this.ParentPositionOffset.Serialize(output, endianess);
			output.WriteValueB32(this.ConsumeUpwards, endianess);
			output.WriteValueF32(this.BoneSweepConstant, endianess);
			output.WriteValueB32(this.TransformationConsume, endianess);
			output.WriteValueF32(this.TransformationConsumeTime, endianess);
			output.WriteValueB32(this.Stealthy, endianess);
			output.WriteValueU64(this.ConsumableProperties, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.RagdollIfNeeded = input.ReadValueU64(endianess);
			this.SwitchToRagdollMode = input.ReadValueB32(endianess);
			this.ParentObjectName = input.ReadValueU64(endianess);
			this.ParentDestinationJoint = input.ReadValueU64(endianess);
			this.ParentPositionOffset = new Vector(input, endianess);
			this.ConsumeUpwards = input.ReadValueB32(endianess);
			this.BoneSweepConstant = input.ReadValueF32(endianess);
			this.TransformationConsume = input.ReadValueB32(endianess);
			this.TransformationConsumeTime = input.ReadValueF32(endianess);
			this.Stealthy = input.ReadValueB32(endianess);
			this.ConsumableProperties = input.ReadValueU64(endianess);
		}
	}
}
