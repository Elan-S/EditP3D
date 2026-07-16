using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14657385042073036494UL)]
	public class AttachObjectTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float BlendDuration { get; set; }
		public ulong ParentName { get; set; }
		public ulong ParentJointName { get; set; }
		public Vector ParentOffset { get; set; } = new Vector();
		public ulong ObjectToAttach { get; set; }
		public ulong ChildJointName { get; set; }
		public Vector ChildOffset { get; set; } = new Vector();
		public Vector ChildOrientation { get; set; } = new Vector();
		public bool UsePhysics { get; set; }
		public PhysicsMode ModeIfUsingPhysics { get; set; }
		public AttachObjectTrack.CharacterModeType CharacterModeIfSimulated { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.BlendDuration, endianess);
			output.WriteValueU64(this.ParentName, endianess);
			output.WriteValueU64(this.ParentJointName, endianess);
			this.ParentOffset.Serialize(output, endianess);
			output.WriteValueU64(this.ObjectToAttach, endianess);
			output.WriteValueU64(this.ChildJointName, endianess);
			this.ChildOffset.Serialize(output, endianess);
			this.ChildOrientation.Serialize(output, endianess);
			output.WriteValueB32(this.UsePhysics, endianess);
			BaseProperty.SerializePropertyEnum<PhysicsMode>(output, endianess, this.ModeIfUsingPhysics);
			BaseProperty.SerializePropertyEnum<AttachObjectTrack.CharacterModeType>(output, endianess, this.CharacterModeIfSimulated);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.BlendDuration = input.ReadValueF32(endianess);
			this.ParentName = input.ReadValueU64(endianess);
			this.ParentJointName = input.ReadValueU64(endianess);
			this.ParentOffset.Deserialize(input, endianess);
			this.ObjectToAttach = input.ReadValueU64(endianess);
			this.ChildJointName = input.ReadValueU64(endianess);
			this.ChildOffset.Deserialize(input, endianess);
			this.ChildOrientation.Deserialize(input, endianess);
			this.UsePhysics = input.ReadValueB32(endianess);
			this.ModeIfUsingPhysics = BaseProperty.DeserializePropertyEnum<PhysicsMode>(input, endianess);
			this.CharacterModeIfSimulated = BaseProperty.DeserializePropertyEnum<AttachObjectTrack.CharacterModeType>(input, endianess);
		}
		public enum CharacterModeType : ulong
		{
			Unsupported = 5579534211636476125UL,
			Supported = 13080926986062682178UL,
			Locomoting = 16331038302739447885UL
		}
	}
}
