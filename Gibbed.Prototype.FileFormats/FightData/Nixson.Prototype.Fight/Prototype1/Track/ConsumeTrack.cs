using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13359649585499843614UL)]
	public class ConsumeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong GrabSlot { get; set; }
		public bool Stealthy { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueB32(this.Stealthy, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Stealthy = input.ReadValueB32(endianess);
		}
	}
}
