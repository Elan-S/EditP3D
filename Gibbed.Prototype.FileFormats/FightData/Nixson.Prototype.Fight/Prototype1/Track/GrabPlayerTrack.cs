using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.GrabPlayer)]
	public class GrabPlayerTrack : P1Track
	{
		public bool UseLocalSpace { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public PlayerType GrabbingPlayer { get; set; }
		public PlayerType GrabbedPlayer { get; set; }
		public GrabPlayerTrack.ActionOnEndType ActionOnEnd { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong ParentJoint { get; set; }
		public Vector ParentPositionOffset { get; set; } = new Vector();
		public Vector ParentRotationOffset { get; set; } = new Vector();
		public ulong ChildJoint { get; set; }
		public Vector ChildPositionOffset { get; set; } = new Vector();
		public Vector ChildRotationOffset { get; set; } = new Vector();
		public float BlendTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.UseLocalSpace, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.GrabbingPlayer);
			BaseProperty.SerializePropertyEnum<PlayerType>(output, endianess, this.GrabbedPlayer);
			BaseProperty.SerializePropertyEnum<GrabPlayerTrack.ActionOnEndType>(output, endianess, this.ActionOnEnd);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.ParentJoint, endianess);
			this.ParentPositionOffset.Serialize(output, endianess);
			this.ParentRotationOffset.Serialize(output, endianess);
			output.WriteValueU64(this.ChildJoint, endianess);
			this.ChildPositionOffset.Serialize(output, endianess);
			this.ChildRotationOffset.Serialize(output, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.UseLocalSpace = input.ReadValueB32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.GrabbingPlayer = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
			this.GrabbedPlayer = BaseProperty.DeserializePropertyEnum<PlayerType>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<GrabPlayerTrack.ActionOnEndType>(input, endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.ParentJoint = input.ReadValueU64(endianess);
			this.ParentPositionOffset.Deserialize(input, endianess);
			this.ParentRotationOffset.Deserialize(input, endianess);
			this.ChildJoint = input.ReadValueU64(endianess);
			this.ChildPositionOffset.Deserialize(input, endianess);
			this.ChildRotationOffset.Deserialize(input, endianess);
			this.BlendTime = input.ReadValueF32(endianess);
		}
		public enum ActionOnEndType : ulong
		{
			Nothing = 2101550166651037979UL,
			Detach = 6128845052282864129UL
		}
	}
}
