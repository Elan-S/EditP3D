using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(10188051947612843482UL)]
	public class AutoTargetingCondition : P1Condition
	{
		public bool IsAutoTargeting { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsAutoTargeting, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsAutoTargeting = input.ReadValueB32(endianess);
		}
	}
}
