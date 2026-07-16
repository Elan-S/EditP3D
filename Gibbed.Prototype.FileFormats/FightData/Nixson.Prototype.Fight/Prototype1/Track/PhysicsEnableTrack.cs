using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10049259146213387470UL)]
	public class PhysicsEnableTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public PhysicsEnableTrack.PhysicsEnableActionType ActionOnBegin { get; set; }
		public PhysicsEnableTrack.PhysicsEnableActionType ActionOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<PhysicsEnableTrack.PhysicsEnableActionType>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<PhysicsEnableTrack.PhysicsEnableActionType>(output, endianess, this.ActionOnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<PhysicsEnableTrack.PhysicsEnableActionType>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<PhysicsEnableTrack.PhysicsEnableActionType>(input, endianess);
		}
		public enum PhysicsEnableActionType : ulong
		{
			DoNothing = 3876518407870578744UL,
			RestorePrevious = 6206664339066247152UL,
			EnablePhysics = 1538342485506640358UL,
			DisablePhysics = 4359553660366836495UL
		}
	}
}
