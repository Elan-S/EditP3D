using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11244462409414368650UL)]
	public class GrabbedSwapGrabberSlotsTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong SlotAHash { get; set; }
		public ulong SlotBHash { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.SlotAHash, endianess);
			output.WriteValueU64(this.SlotBHash, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.SlotAHash = input.ReadValueU64(endianess);
			this.SlotBHash = input.ReadValueU64(endianess);
		}
	}
}
