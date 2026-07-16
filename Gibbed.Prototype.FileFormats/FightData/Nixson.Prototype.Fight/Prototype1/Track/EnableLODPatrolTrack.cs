using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.EnableLODPatrol)]
	public class EnableLODPatrolTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public EnableLODPatrolTrack.EventPatrolType ActionOnBegin { get; set; }
		public EnableLODPatrolTrack.EventPatrolType ActionOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<EnableLODPatrolTrack.EventPatrolType>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<EnableLODPatrolTrack.EventPatrolType>(output, endianess, this.ActionOnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<EnableLODPatrolTrack.EventPatrolType>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<EnableLODPatrolTrack.EventPatrolType>(input, endianess);
		}
		public enum EventPatrolType : ulong
		{
			DoNothing = 3876518407870578744UL,
			RestorePrevious = 6206664339066247152UL,
			EnablePatrol = 5855201438174134505UL,
			DisablePatrol = 3863038993445046482UL
		}
	}
}
