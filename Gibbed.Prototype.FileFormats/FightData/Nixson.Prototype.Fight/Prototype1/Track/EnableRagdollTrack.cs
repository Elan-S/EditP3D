using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(17208543793389083238UL)]
	public class EnableRagdollTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool DisableSupportingLimb { get; set; }
		public bool InheritJointVelocities { get; set; }
		public bool UseNormalPhysicsObjectAsRagdoll { get; set; }
		public float PhysicsTransitionDuration { get; set; }
		public ulong RootJoint { get; set; }
		public bool IncludeRootJoint { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.DisableSupportingLimb, endianess);
			output.WriteValueB32(this.InheritJointVelocities, endianess);
			output.WriteValueB32(this.UseNormalPhysicsObjectAsRagdoll, endianess);
			output.WriteValueF32(this.PhysicsTransitionDuration, endianess);
			output.WriteValueU64(this.RootJoint, endianess);
			output.WriteValueB32(this.IncludeRootJoint, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.DisableSupportingLimb = input.ReadValueB32(endianess);
			this.InheritJointVelocities = input.ReadValueB32(endianess);
			this.UseNormalPhysicsObjectAsRagdoll = input.ReadValueB32(endianess);
			this.PhysicsTransitionDuration = input.ReadValueF32(endianess);
			this.RootJoint = input.ReadValueU64(endianess);
			this.IncludeRootJoint = input.ReadValueB32(endianess);
		}
	}
}
