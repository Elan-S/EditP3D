using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(12081921791072946730UL)]
	public class InBlastAreaCondition : P1Condition
	{
		public bool UseDestination { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.UseDestination, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.UseDestination = input.ReadValueB32(endianess);
		}
	}
}
