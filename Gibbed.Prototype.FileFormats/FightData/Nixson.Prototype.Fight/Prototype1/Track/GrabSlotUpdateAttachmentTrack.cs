using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11773931617089059926UL)]
	public class GrabSlotUpdateAttachmentTrack : P1Track
	{
		public float BeginTime { get; set; }
		public ulong GrabSlotHash { get; set; }
		public ulong ParentJointHash { get; set; }
		public bool OverrideParentTranslationJoint { get; set; }
		public ulong ParentTranslationJoint { get; set; }
		public float ParentPositionOffsetX { get; set; }
		public float ParentPositionOffsetY { get; set; }
		public float ParentPositionOffsetZ { get; set; }
		public float ParentRotationOffsetX { get; set; }
		public float ParentRotationOffsetY { get; set; }
		public float ParentRotationOffsetZ { get; set; }
		public ulong ChildJointHash { get; set; }
		public bool OverrideChildTranslationJoint { get; set; }
		public ulong ChildTranslationJointHash { get; set; }
		public float ChildPositionOffsetX { get; set; }
		public float ChildPositionOffsetY { get; set; }
		public float ChildPositionOffsetZ { get; set; }
		public float ChildRotationOffsetX { get; set; }
		public float ChildRotationOffsetY { get; set; }
		public float ChildRotationOffsetZ { get; set; }
		public bool ConsiderChildAlternateRotation { get; set; }
		public float ChildAlternateRotationOffsetX { get; set; }
		public float ChildAlternateRotationOffsetY { get; set; }
		public float ChildAlternateRotationOffsetZ { get; set; }
		public float BlendTime { get; set; }
		public PhysicsMode PhysicsMode { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueU64(this.GrabSlotHash, endianess);
			output.WriteValueU64(this.ParentJointHash, endianess);
			output.WriteValueB32(this.OverrideParentTranslationJoint, endianess);
			output.WriteValueU64(this.ParentTranslationJoint, endianess);
			output.WriteValueF32(this.ParentPositionOffsetX, endianess);
			output.WriteValueF32(this.ParentPositionOffsetY, endianess);
			output.WriteValueF32(this.ParentPositionOffsetZ, endianess);
			output.WriteValueF32(this.ParentRotationOffsetX, endianess);
			output.WriteValueF32(this.ParentRotationOffsetY, endianess);
			output.WriteValueF32(this.ParentRotationOffsetZ, endianess);
			output.WriteValueU64(this.ChildJointHash, endianess);
			output.WriteValueB32(this.OverrideChildTranslationJoint, endianess);
			output.WriteValueU64(this.ChildTranslationJointHash, endianess);
			output.WriteValueF32(this.ChildPositionOffsetX, endianess);
			output.WriteValueF32(this.ChildPositionOffsetY, endianess);
			output.WriteValueF32(this.ChildPositionOffsetZ, endianess);
			output.WriteValueF32(this.ChildRotationOffsetX, endianess);
			output.WriteValueF32(this.ChildRotationOffsetY, endianess);
			output.WriteValueF32(this.ChildRotationOffsetZ, endianess);
			output.WriteValueB32(this.ConsiderChildAlternateRotation, endianess);
			output.WriteValueF32(this.ChildAlternateRotationOffsetX, endianess);
			output.WriteValueF32(this.ChildAlternateRotationOffsetY, endianess);
			output.WriteValueF32(this.ChildAlternateRotationOffsetZ, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
			BaseProperty.SerializePropertyEnum<PhysicsMode>(output, endianess, this.PhysicsMode);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.GrabSlotHash = input.ReadValueU64(endianess);
			this.ParentJointHash = input.ReadValueU64(endianess);
			this.OverrideParentTranslationJoint = input.ReadValueB32(endianess);
			this.ParentTranslationJoint = input.ReadValueU64(endianess);
			this.ParentPositionOffsetX = input.ReadValueF32(endianess);
			this.ParentPositionOffsetY = input.ReadValueF32(endianess);
			this.ParentPositionOffsetZ = input.ReadValueF32(endianess);
			this.ParentRotationOffsetX = input.ReadValueF32(endianess);
			this.ParentRotationOffsetY = input.ReadValueF32(endianess);
			this.ParentRotationOffsetZ = input.ReadValueF32(endianess);
			this.ChildJointHash = input.ReadValueU64(endianess);
			this.OverrideChildTranslationJoint = input.ReadValueB32(endianess);
			this.ChildTranslationJointHash = input.ReadValueU64(endianess);
			this.ChildPositionOffsetX = input.ReadValueF32(endianess);
			this.ChildPositionOffsetY = input.ReadValueF32(endianess);
			this.ChildPositionOffsetZ = input.ReadValueF32(endianess);
			this.ChildRotationOffsetX = input.ReadValueF32(endianess);
			this.ChildRotationOffsetY = input.ReadValueF32(endianess);
			this.ChildRotationOffsetZ = input.ReadValueF32(endianess);
			this.ConsiderChildAlternateRotation = input.ReadValueB32(endianess);
			this.ChildAlternateRotationOffsetX = input.ReadValueF32(endianess);
			this.ChildAlternateRotationOffsetY = input.ReadValueF32(endianess);
			this.ChildAlternateRotationOffsetZ = input.ReadValueF32(endianess);
			this.BlendTime = input.ReadValueF32(endianess);
			this.PhysicsMode = BaseProperty.DeserializePropertyEnum<PhysicsMode>(input, endianess);
		}
	}
}
