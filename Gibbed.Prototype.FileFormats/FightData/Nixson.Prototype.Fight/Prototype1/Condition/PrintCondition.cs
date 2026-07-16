using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Print)]
	public class PrintCondition : P1Condition
	{
		public string Channel { get; set; }
		public string Text { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteStringAlignedU32(this.Channel, endianess);
			output.WriteStringAlignedU32(this.Text, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Channel = input.ReadStringAlignedU32(endianess);
			this.Text = input.ReadStringAlignedU32(endianess);
		}
	}
}
