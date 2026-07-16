using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(15623637673121890617UL)]
	public class ReactionHitFromMuscleMassCondition : P1Condition
	{
		public bool MuscleMassActive { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.MuscleMassActive, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.MuscleMassActive = input.ReadValueB32(endianess);
		}
	}
}
