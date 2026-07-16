using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(15211016316895126960UL)]
	public class ConsumptionNameCondition : P1Condition
	{
		public ulong Name { get; set; }
		public bool IsActive { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Name, endianess);
			output.WriteValueB32(this.IsActive, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.IsActive = input.ReadValueB32(endianess);
		}
	}
}
