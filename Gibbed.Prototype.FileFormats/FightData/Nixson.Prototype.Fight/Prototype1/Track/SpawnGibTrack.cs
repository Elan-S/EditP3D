using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(18267164372812551285UL)]
	public class SpawnGibTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong GibName { get; set; }
		public bool AddSuffix { get; set; }
		public ulong TemplateName { get; set; }
		public bool CopyShader { get; set; }
		public ulong Joint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Rotation { get; set; } = new Vector();
		public bool AttachToParent { get; set; }
		public ulong GrabSlot { get; set; }
		public bool InheritPose { get; set; }
		public bool AttachToGrandParent { get; set; }
		public float ImpulseScale { get; set; }
		public bool TransferTargetLocks { get; set; }
		public bool CopyTranformationDescription { get; set; }
		public bool UseParentExclusionGroup { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.GibName, endianess);
			output.WriteValueB32(this.AddSuffix, endianess);
			output.WriteValueU64(this.TemplateName, endianess);
			output.WriteValueB32(this.CopyShader, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Rotation.Serialize(output, endianess);
			output.WriteValueB32(this.AttachToParent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueB32(this.InheritPose, endianess);
			output.WriteValueB32(this.AttachToGrandParent, endianess);
			output.WriteValueF32(this.ImpulseScale, endianess);
			output.WriteValueB32(this.TransferTargetLocks, endianess);
			output.WriteValueB32(this.CopyTranformationDescription, endianess);
			output.WriteValueB32(this.UseParentExclusionGroup, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.GibName = input.ReadValueU64(endianess);
			this.AddSuffix = input.ReadValueB32(endianess);
			this.TemplateName = input.ReadValueU64(endianess);
			this.CopyShader = input.ReadValueB32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.Rotation = new Vector(input, endianess);
			this.AttachToParent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.InheritPose = input.ReadValueB32(endianess);
			this.AttachToGrandParent = input.ReadValueB32(endianess);
			this.ImpulseScale = input.ReadValueF32(endianess);
			this.TransferTargetLocks = input.ReadValueB32(endianess);
			this.CopyTranformationDescription = input.ReadValueB32(endianess);
			this.UseParentExclusionGroup = input.ReadValueB32(endianess);
		}
	}
}
