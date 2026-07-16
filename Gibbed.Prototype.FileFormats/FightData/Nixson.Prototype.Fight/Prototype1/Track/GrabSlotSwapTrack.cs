using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14153513724761592955UL)]
	public class GrabSlotSwapTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong SlotA { get; set; }
		public ulong SlotB { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.SlotA, endianess);
			output.WriteValueU64(this.SlotB, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.SlotA = input.ReadValueU64(endianess);
			this.SlotB = input.ReadValueU64(endianess);
		}
	}
}
