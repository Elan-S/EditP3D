using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12513621923296063815UL)]
	public class AIMessageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public MessageType Type { get; set; }
		public float Intensity { get; set; }
		public TakerGiverType Taker { get; set; }
		public bool UseOriginator { get; set; }
		public ulong GrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<MessageType>(output, endianess, this.Type);
			output.WriteValueF32(this.Intensity, endianess);
			BaseProperty.SerializePropertyEnum<TakerGiverType>(output, endianess, this.Taker);
			output.WriteValueB32(this.UseOriginator, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<MessageType>(input, endianess);
			this.Intensity = input.ReadValueF32(endianess);
			this.Taker = BaseProperty.DeserializePropertyEnum<TakerGiverType>(input, endianess);
			this.UseOriginator = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
	}
}
