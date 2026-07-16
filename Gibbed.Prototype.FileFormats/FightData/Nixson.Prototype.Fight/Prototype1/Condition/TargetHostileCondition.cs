using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.TargetHostile)]
	public class TargetHostileCondition : P1Condition
	{
		public bool IsHostile { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsHostile, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsHostile = input.ReadValueB32(endianess);
		}
	}
}
