using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.NPCAnimationVersion)]
	public class NPCAnimationVersionCondition : P1Condition
	{
		public bool Match { get; set; }
		public int Version { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Match, endianess);
			output.WriteValueS32(this.Version, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Match = input.ReadValueB32(endianess);
			this.Version = input.ReadValueS32(endianess);
		}
	}
}
