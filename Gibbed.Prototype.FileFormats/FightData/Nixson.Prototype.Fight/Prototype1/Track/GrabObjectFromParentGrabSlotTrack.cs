using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11217986622204522521UL)]
	public class GrabObjectFromParentGrabSlotTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong ParentSlot { get; set; }
		public ulong ChildSlot { get; set; }
		public int InterruptPriority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.ParentSlot, endianess);
			output.WriteValueU64(this.ChildSlot, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ParentSlot = input.ReadValueU64(endianess);
			this.ChildSlot = input.ReadValueU64(endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
		}
	}
}
