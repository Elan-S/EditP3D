using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.PlayerSeen)]
	public class PlayerSeenCondition : P1Condition
	{
		public bool Seen { get; set; }
		public CompareOperator Since { get; set; }
		public float Time { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Seen, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Since);
			output.WriteValueF32(this.Time, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Seen = input.ReadValueB32(endianess);
			this.Since = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Time = input.ReadValueF32(endianess);
		}
	}
}
