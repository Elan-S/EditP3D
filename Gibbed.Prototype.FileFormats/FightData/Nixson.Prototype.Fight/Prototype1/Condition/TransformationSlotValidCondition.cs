using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(13755595673288793299UL)]
	public class TransformationSlotValidCondition : P1Condition
	{
		public ulong SlotName { get; set; }
		public bool IsValid { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.SlotName, endianess);
			output.WriteValueB32(this.IsValid, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SlotName = input.ReadValueU64(endianess);
			this.IsValid = input.ReadValueB32(endianess);
		}
	}
}
