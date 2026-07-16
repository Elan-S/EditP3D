using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.DebugLayer)]
	public class DebugLayerCondition : P1Condition
	{
		public string DebugLayer { get; set; }
		public bool Enabled { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteStringAlignedU32(this.DebugLayer, endianess);
			output.WriteValueB32(this.Enabled, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.DebugLayer = input.ReadStringAlignedU32(endianess);
			this.Enabled = input.ReadValueB32(endianess);
		}
	}
}
