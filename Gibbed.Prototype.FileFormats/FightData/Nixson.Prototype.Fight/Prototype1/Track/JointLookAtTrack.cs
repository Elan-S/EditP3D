using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11693099025353764666UL)]
	public class JointLookAtTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Joint { get; set; }
		public Vector Heading { get; set; } = new Vector();
		public Vector PrimaryRotationAxis { get; set; } = new Vector();
		public float PrimaryTurnRate { get; set; }
		public float PrimaryAngleMin { get; set; }
		public float PrimaryAngleMax { get; set; }
		public float SecondaryTurnRate { get; set; }
		public float SecondaryAngleMin { get; set; }
		public float SecondaryAngleMax { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public ulong UseGrabSlot { get; set; }
		public LookAtType Where { get; set; }
		public bool UseAutoTarget { get; set; }
		public bool UseIntentionWhenNoTarget { get; set; }
		public bool UseFreeAimWhenNoTarget { get; set; }
		public bool UseLocalAngelsInIntention { get; set; }
		public Vector TargetOffset { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Heading.Serialize(output, endianess);
			this.PrimaryRotationAxis.Serialize(output, endianess);
			output.WriteValueF32(this.PrimaryTurnRate, endianess);
			output.WriteValueF32(this.PrimaryAngleMin, endianess);
			output.WriteValueF32(this.PrimaryAngleMax, endianess);
			output.WriteValueF32(this.SecondaryTurnRate, endianess);
			output.WriteValueF32(this.SecondaryAngleMin, endianess);
			output.WriteValueF32(this.SecondaryAngleMax, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			output.WriteValueU64(this.UseGrabSlot, endianess);
			BaseProperty.SerializePropertyEnum<LookAtType>(output, endianess, this.Where);
			output.WriteValueB32(this.UseAutoTarget, endianess);
			output.WriteValueB32(this.UseIntentionWhenNoTarget, endianess);
			output.WriteValueB32(this.UseFreeAimWhenNoTarget, endianess);
			output.WriteValueB32(this.UseLocalAngelsInIntention, endianess);
			this.TargetOffset.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Heading = new Vector(input, endianess);
			this.PrimaryRotationAxis = new Vector(input, endianess);
			this.PrimaryTurnRate = input.ReadValueF32(endianess);
			this.PrimaryAngleMin = input.ReadValueF32(endianess);
			this.PrimaryAngleMax = input.ReadValueF32(endianess);
			this.SecondaryTurnRate = input.ReadValueF32(endianess);
			this.SecondaryAngleMin = input.ReadValueF32(endianess);
			this.SecondaryAngleMax = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.UseGrabSlot = input.ReadValueU64(endianess);
			this.Where = BaseProperty.DeserializePropertyEnum<LookAtType>(input, endianess);
			this.UseAutoTarget = input.ReadValueB32(endianess);
			this.UseIntentionWhenNoTarget = input.ReadValueB32(endianess);
			this.UseFreeAimWhenNoTarget = input.ReadValueB32(endianess);
			this.UseLocalAngelsInIntention = input.ReadValueB32(endianess);
			this.TargetOffset = new Vector(input, endianess);
		}
	}
}
