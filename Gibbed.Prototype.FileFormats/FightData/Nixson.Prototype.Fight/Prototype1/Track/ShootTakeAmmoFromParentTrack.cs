using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ShootTakeAmmoFromParent)]
	public class ShootTakeAmmoFromParentTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong WeaponGrabSlot { get; set; }
		public ulong ReceiverGrabSlot { get; set; }
		public ulong ReceiverJoint { get; set; }
		public Vector ReceiverPositionOffset { get; set; } = new Vector();
		public Vector ReceiverRotationOffset { get; set; } = new Vector();
		public ulong ChildJoint { get; set; }
		public Vector ChildPositionOffset { get; set; } = new Vector();
		public Vector ChildRotationOffset { get; set; } = new Vector();
		public float BlendTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.WeaponGrabSlot, endianess);
			output.WriteValueU64(this.ReceiverGrabSlot, endianess);
			output.WriteValueU64(this.ReceiverJoint, endianess);
			this.ReceiverPositionOffset.Serialize(output, endianess);
			this.ReceiverRotationOffset.Serialize(output, endianess);
			output.WriteValueU64(this.ChildJoint, endianess);
			this.ChildPositionOffset.Serialize(output, endianess);
			this.ChildRotationOffset.Serialize(output, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.WeaponGrabSlot = input.ReadValueU64(endianess);
			this.ReceiverGrabSlot = input.ReadValueU64(endianess);
			this.ReceiverJoint = input.ReadValueU64(endianess);
			this.ReceiverPositionOffset.Deserialize(input, endianess);
			this.ReceiverRotationOffset.Deserialize(input, endianess);
			this.ChildJoint = input.ReadValueU64(endianess);
			this.ChildPositionOffset.Deserialize(input, endianess);
			this.ChildRotationOffset.Deserialize(input, endianess);
			this.BlendTime = input.ReadValueF32(endianess);
		}
	}
}
