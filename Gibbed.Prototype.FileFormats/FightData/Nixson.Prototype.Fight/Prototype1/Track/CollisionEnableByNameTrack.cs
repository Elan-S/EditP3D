using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.CollisionEnableByName)]
	public class CollisionEnableByNameTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong CollisionObject { get; set; }
		public CollisionEnableByNameTrack.EventType ActionOnBegin { get; set; }
		public CollisionEnableByNameTrack.EventType ActionOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.CollisionObject, endianess);
			BaseProperty.SerializePropertyEnum<CollisionEnableByNameTrack.EventType>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<CollisionEnableByNameTrack.EventType>(output, endianess, this.ActionOnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.CollisionObject = input.ReadValueU64(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<CollisionEnableByNameTrack.EventType>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<CollisionEnableByNameTrack.EventType>(input, endianess);
		}
		public enum EventType : ulong
		{
			DoNothing = 3876518407870578744UL,
			RestoreToPrevious = 6206664339066247152UL,
			EnableCollision = 12870800434117538617UL,
			DisableCollision = 16309147254971539904UL
		}
	}
}
