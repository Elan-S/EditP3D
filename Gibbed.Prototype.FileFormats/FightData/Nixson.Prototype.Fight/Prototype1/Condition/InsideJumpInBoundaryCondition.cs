using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.InsideJumpInBoundary)]
	public class InsideJumpInBoundaryCondition : P1Condition
	{
		public bool InsideJumpInBoundary { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.InsideJumpInBoundary, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.InsideJumpInBoundary = input.ReadValueB32(endianess);
		}
	}
}
