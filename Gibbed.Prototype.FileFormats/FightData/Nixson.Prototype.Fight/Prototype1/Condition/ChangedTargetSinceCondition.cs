using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(12836394523898845507UL)]
	public class ChangedTargetSinceCondition : P1Condition
	{
		public ulong Timer { get; set; }
		public float Time { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Timer, endianess);
			output.WriteValueF32(this.Time, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Timer = input.ReadValueU64(endianess);
			this.Time = input.ReadValueF32(endianess);
		}
	}
}
