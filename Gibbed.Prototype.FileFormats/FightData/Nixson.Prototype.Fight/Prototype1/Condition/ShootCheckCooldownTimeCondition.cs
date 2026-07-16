using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.ShootCheckCooldownTime)]
	public class ShootCheckCooldownTimeCondition : P1Condition
	{
		public bool TestOnParent { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong WeaponEntry { get; set; }
		public CompareOperator Compare { get; set; }
		public float RequiredTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.TestOnParent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.WeaponEntry, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.RequiredTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TestOnParent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.WeaponEntry = input.ReadValueU64(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.RequiredTime = input.ReadValueF32(endianess);
		}
	}
}
