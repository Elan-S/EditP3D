using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(11935830607375375599UL)]
	public class SupportingSurfaceSupportingLimbCondition : P1Condition
	{
		public ulong SupportingLimb { get; set; }
		public bool IsSupportingLimb { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.SupportingLimb, endianess);
			output.WriteValueB32(this.IsSupportingLimb, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SupportingLimb = input.ReadValueU64(endianess);
			this.IsSupportingLimb = input.ReadValueB32(endianess);
		}
	}
}
