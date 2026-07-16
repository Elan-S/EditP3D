using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.CheckTagAvailableSpace)]
	public class CheckTagAvailableSpaceCondition : P1Condition
	{
		public ulong Tag { get; set; }
		public int Limit { get; set; }
		public bool TryDespawnIfFull { get; set; }
		public float MinDespawnDistance { get; set; }
		public float ObjRadius { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Tag, endianess);
			output.WriteValueS32(this.Limit, endianess);
			output.WriteValueB32(this.TryDespawnIfFull, endianess);
			output.WriteValueF32(this.MinDespawnDistance, endianess);
			output.WriteValueF32(this.ObjRadius, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Tag = input.ReadValueU64(endianess);
			this.Limit = input.ReadValueS32(endianess);
			this.TryDespawnIfFull = input.ReadValueB32(endianess);
			this.MinDespawnDistance = input.ReadValueF32(endianess);
			this.ObjRadius = input.ReadValueF32(endianess);
		}
	}
}
