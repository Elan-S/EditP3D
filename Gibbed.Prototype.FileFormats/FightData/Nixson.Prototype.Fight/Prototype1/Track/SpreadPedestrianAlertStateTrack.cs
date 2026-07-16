using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SpreadPedestrianAlertState)]
	public class SpreadPedestrianAlertStateTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public MessageType State { get; set; }
		public float Radius { get; set; }
		public float MaxDistanceFromSource { get; set; }
		public int MaxPasses { get; set; }
		public int AlertFrequency { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<MessageType>(output, endianess, this.State);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.MaxDistanceFromSource, endianess);
			output.WriteValueS32(this.MaxPasses, endianess);
			output.WriteValueS32(this.AlertFrequency, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.State = BaseProperty.DeserializePropertyEnum<MessageType>(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.MaxDistanceFromSource = input.ReadValueF32(endianess);
			this.MaxPasses = input.ReadValueS32(endianess);
			this.AlertFrequency = input.ReadValueS32(endianess);
		}
	}
}
