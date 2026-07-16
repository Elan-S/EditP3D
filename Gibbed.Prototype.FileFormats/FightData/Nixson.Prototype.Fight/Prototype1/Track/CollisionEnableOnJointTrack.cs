using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10429418380169374182UL)]
	public class CollisionEnableOnJointTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong SimulatedJoint { get; set; }
		public CollisionEnableOnJointTrack.OnBeginType ActionOnBegin { get; set; }
		public CollisionEnableOnJointTrack.OnEndType ActionOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.SimulatedJoint, endianess);
			BaseProperty.SerializePropertyEnum<CollisionEnableOnJointTrack.OnBeginType>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<CollisionEnableOnJointTrack.OnEndType>(output, endianess, this.ActionOnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.SimulatedJoint = input.ReadValueU64(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<CollisionEnableOnJointTrack.OnBeginType>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<CollisionEnableOnJointTrack.OnEndType>(input, endianess);
		}
		public enum OnBeginType : ulong
		{
			DoNothing = 3876518407870578744UL,
			EnableCollision = 12870800434117538617UL,
			DisableCollision = 16309147254971539904UL
		}
		public enum OnEndType : ulong
		{
			DoNothing = 3876518407870578744UL,
			RestoreToPrevious = 6206664339066247152UL,
			EnableCollision = 12870800434117538617UL,
			DisableCollision = 16309147254971539904UL
		}
	}
}
