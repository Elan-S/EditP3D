using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Grab)]
	public class CollisionGrabTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong Joint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public float Radius { get; set; }
		public bool UseMotion { get; set; }
		public float ArcOffset { get; set; }
		public float ArcRange { get; set; }
		public Collidables CollideWith { get; set; }
		public bool CheckForObstruction { get; set; }
		public bool GrabOnlyPowerTarget { get; set; }
		public bool ConsiderPlatformAsTarget { get; set; }
		public bool ReverseAttachOnMountable { get; set; }
		public BranchReference GiverBranch { get; set; } = new BranchReference();
		public BranchReference ReceiverBranch { get; set; } = new BranchReference();
		public int InterruptPriority { get; set; }
		public bool DevastatorGrab { get; set; }
		public PhysicsMode PhysicsMode { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueB32(this.UseMotion, endianess);
			output.WriteValueF32(this.ArcOffset, endianess);
			output.WriteValueF32(this.ArcRange, endianess);
			BaseProperty.SerializePropertyBitfield<Collidables>(output, endianess, this.CollideWith);
			output.WriteValueB32(this.CheckForObstruction, endianess);
			output.WriteValueB32(this.GrabOnlyPowerTarget, endianess);
			output.WriteValueB32(this.ConsiderPlatformAsTarget, endianess);
			output.WriteValueB32(this.ReverseAttachOnMountable, endianess);
			this.GiverBranch.Serialize(output, endianess);
			this.ReceiverBranch.Serialize(output, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
			output.WriteValueB32(this.DevastatorGrab, endianess);
			BaseProperty.SerializePropertyEnum<PhysicsMode>(output, endianess, this.PhysicsMode);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.UseMotion = input.ReadValueB32(endianess);
			this.ArcOffset = input.ReadValueF32(endianess);
			this.ArcRange = input.ReadValueF32(endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<Collidables>(input, endianess);
			this.CheckForObstruction = input.ReadValueB32(endianess);
			this.GrabOnlyPowerTarget = input.ReadValueB32(endianess);
			this.ConsiderPlatformAsTarget = input.ReadValueB32(endianess);
			this.ReverseAttachOnMountable = input.ReadValueB32(endianess);
			this.GiverBranch = new BranchReference(input, endianess);
			this.ReceiverBranch = new BranchReference(input, endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
			this.DevastatorGrab = input.ReadValueB32(endianess);
			this.PhysicsMode = BaseProperty.DeserializePropertyEnum<PhysicsMode>(input, endianess);
		}
	}
}
