using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(17162451314752661093UL)]
	public class TargetingCondition : P1Condition
	{
		public bool IsTargeting { get; set; }
		public bool IncludeAutoTarget { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsTargeting, endianess);
			output.WriteValueB32(this.IncludeAutoTarget, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsTargeting = input.ReadValueB32(endianess);
			this.IncludeAutoTarget = input.ReadValueB32(endianess);
		}
	}
}
