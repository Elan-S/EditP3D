using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11774275985657976411UL)]
	public class HitWeaponTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong GrabSlot { get; set; }
		public int CollisionCountMax { get; set; }
		public float Damage { get; set; }
		public DamageType DamageType { get; set; }
		public Vector Impulse { get; set; } = new Vector();
		public BranchReference ReceiverBranch { get; set; } = new BranchReference();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueS32(this.CollisionCountMax, endianess);
			output.WriteValueF32(this.Damage, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			this.Impulse.Serialize(output, endianess);
			this.ReceiverBranch.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.CollisionCountMax = input.ReadValueS32(endianess);
			this.Damage = input.ReadValueF32(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.Impulse.Deserialize(input, endianess);
			this.ReceiverBranch.Deserialize(input, endianess);
		}
	}
}
