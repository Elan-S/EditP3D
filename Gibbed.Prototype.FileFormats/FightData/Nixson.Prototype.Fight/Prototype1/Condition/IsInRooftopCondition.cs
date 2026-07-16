using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(13081676850912719020UL)]
	public class IsInRooftopCondition : P1Condition
	{
		public bool Rooftop { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Rooftop, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Rooftop = input.ReadValueB32(endianess);
		}
	}
}
