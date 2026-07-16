using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(16769377021546197278UL)]
	public class IsMissileTargetingPlayerCondition : P1Condition
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
