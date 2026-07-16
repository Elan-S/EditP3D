using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Blind)]
	public class BlindTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public TakerGiverType Taker { get; set; }
		public bool Blind { get; set; }
		public ulong GrabSlot { get; set; }
		public bool UndoOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<TakerGiverType>(output, endianess, this.Taker);
			output.WriteValueB32(this.Blind, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueB32(this.UndoOnEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Taker = BaseProperty.DeserializePropertyEnum<TakerGiverType>(input, endianess);
			this.Blind = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.UndoOnEnd = input.ReadValueB32(endianess);
		}
	}
}
