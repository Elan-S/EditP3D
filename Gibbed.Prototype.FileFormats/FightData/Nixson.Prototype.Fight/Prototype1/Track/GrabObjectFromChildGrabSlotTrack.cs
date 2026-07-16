using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(16359396084105925077UL)]
	public class GrabObjectFromChildGrabSlotTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Child { get; set; }
		public ulong ChildGrabSlot { get; set; }
		public ulong GrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Child, endianess);
			output.WriteValueU64(this.ChildGrabSlot, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Child = input.ReadValueU64(endianess);
			this.ChildGrabSlot = input.ReadValueU64(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
	}
}
