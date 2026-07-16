using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.SequencerBranch)]
	public class SequencerBranchCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public bool Recurse { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			this.Branch.Serialize(output, endianess);
			output.WriteValueB32(this.Recurse, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Branch = new BranchReference(input, endianess);
			this.Recurse = input.ReadValueB32(endianess);
		}
	}
}
