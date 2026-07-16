using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.GrabbedUpdateAttachment)]
	public class GrabbedUpdateAttachmentTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong ParentJoint { get; set; }
		public Vector ParentPositionOffset { get; set; } = new Vector();
		public bool UpdateRotationOffset { get; set; }
		public Vector ParentRotationOffset { get; set; } = new Vector();
		public ulong ChildJoint { get; set; }
		public Vector ChildPositionOffset { get; set; } = new Vector();
		public Vector ChildRotationOffset { get; set; } = new Vector();
		public float BlendTime { get; set; }
		public PhysicsMode PhysicsMode { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.ParentJoint, endianess);
			this.ParentPositionOffset.Serialize(output, endianess);
			output.WriteValueB32(this.UpdateRotationOffset, endianess);
			this.ParentRotationOffset.Serialize(output, endianess);
			output.WriteValueU64(this.ChildJoint, endianess);
			this.ChildPositionOffset.Serialize(output, endianess);
			this.ChildRotationOffset.Serialize(output, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
			BaseProperty.SerializePropertyEnum<PhysicsMode>(output, endianess, this.PhysicsMode);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ParentJoint = input.ReadValueU64(endianess);
			this.ParentPositionOffset = new Vector(input, endianess);
			this.UpdateRotationOffset = input.ReadValueB32(endianess);
			this.ParentRotationOffset = new Vector(input, endianess);
			this.ChildJoint = input.ReadValueU64(endianess);
			this.ChildPositionOffset = new Vector(input, endianess);
			this.ChildRotationOffset = new Vector(input, endianess);
			this.BlendTime = input.ReadValueF32(endianess);
			this.PhysicsMode = BaseProperty.DeserializePropertyEnum<PhysicsMode>(input, endianess);
		}
	}
}
