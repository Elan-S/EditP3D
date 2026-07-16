using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.TargetCanSee)]
	public class TargetCanSeeCondition : P1Condition
	{
		public bool Seen { get; set; }
		public float Since { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Seen, endianess);
			output.WriteValueF32(this.Since, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Seen = input.ReadValueB32(endianess);
			this.Since = input.ReadValueF32(endianess);
		}
	}
}
