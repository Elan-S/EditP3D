using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(12567948215394369820UL)]
	public class AlertStatusCondition : P1Condition
	{
		public ulong Affiliate { get; set; }
		public bool Alert { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Affiliate, endianess);
			output.WriteValueB32(this.Alert, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Affiliate = input.ReadValueU64(endianess);
			this.Alert = input.ReadValueB32(endianess);
		}
	}
}
