using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11803232558002950326UL)]
	public class FloatShotAllowedCondition : P1Condition
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
