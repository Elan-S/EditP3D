using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(9809208206919308375UL)]
	public class GrabSlotClassNameCondition : P1Condition
	{
		public ulong GrabSlotHash { get; set; }
		public CompareOperator Compare { get; set; }
		public ulong ClassnameHash { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlotHash, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueU64(this.ClassnameHash, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabSlotHash = input.ReadValueU64(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.ClassnameHash = input.ReadValueU64(endianess);
		}
	}
}
