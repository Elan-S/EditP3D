using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Enabled)]
	public class EnabledCondition : P1Condition
	{
		public bool IsEnabled { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsEnabled, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsEnabled = input.ReadValueB32(endianess);
		}
	}
}
