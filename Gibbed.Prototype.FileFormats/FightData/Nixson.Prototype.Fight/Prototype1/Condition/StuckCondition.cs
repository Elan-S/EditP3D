using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Stuck)]
	public class StuckCondition : P1Condition
	{
		public bool Stuck { get; set; }
		public float TimeStuck { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Stuck, endianess);
			output.WriteValueF32(this.TimeStuck, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Stuck = input.ReadValueB32(endianess);
			this.TimeStuck = input.ReadValueF32(endianess);
		}
	}
}
