using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(16145748555512912048UL)]
	public class GrabbedBySlotCondition : P1Condition
	{
		public bool IsGrabbed { get; set; }
		public ulong GrabSlotHash { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsGrabbed);
			output.WriteValueU64(this.GrabSlotHash, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsGrabbed = input.ReadValueB32(endianess);
			this.GrabSlotHash = input.ReadValueU64(endianess);
		}
	}
}
