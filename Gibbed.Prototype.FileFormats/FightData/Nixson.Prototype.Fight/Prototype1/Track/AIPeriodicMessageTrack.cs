using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.AIPeriodicMessage)]
	public class AIPeriodicMessageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public MessageType Type { get; set; }
		public float Intensity { get; set; }
		public TakerGiverType Taker { get; set; }
		public TakerGiverType Giver { get; set; }
		public bool UseOriginator { get; set; }
		public float Period { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<MessageType>(output, endianess, this.Type);
			output.WriteValueF32(this.Intensity, endianess);
			BaseProperty.SerializePropertyEnum<TakerGiverType>(output, endianess, this.Taker);
			BaseProperty.SerializePropertyEnum<TakerGiverType>(output, endianess, this.Giver);
			output.WriteValueB32(this.UseOriginator, endianess);
			output.WriteValueF32(this.Period, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<MessageType>(input, endianess);
			this.Intensity = input.ReadValueF32(endianess);
			this.Taker = BaseProperty.DeserializePropertyEnum<TakerGiverType>(input, endianess);
			this.Giver = BaseProperty.DeserializePropertyEnum<TakerGiverType>(input, endianess);
			this.UseOriginator = input.ReadValueB32(endianess);
			this.Period = input.ReadValueF32(endianess);
		}
	}
}
