using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(14522115098003452540UL)]
	public class RequestDenyPatsyCondition : P1Condition
	{
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
		}
	}
}
