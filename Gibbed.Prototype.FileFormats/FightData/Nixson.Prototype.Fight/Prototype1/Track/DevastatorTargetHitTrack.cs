using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.DevastatorTargetHit)]
	public class DevastatorTargetHitTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool UseAttackTimeBegin { get; set; }
		public bool UseAttackTimeEnd { get; set; }
		public float Damage { get; set; }
		public Vector Impulse { get; set; } = new Vector();
		public AttackType AttackType { get; set; }
		public DamageType DamageType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.UseAttackTimeBegin, endianess);
			output.WriteValueB32(this.UseAttackTimeEnd, endianess);
			output.WriteValueF32(this.Damage, endianess);
			this.Impulse.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.UseAttackTimeBegin = input.ReadValueB32(endianess);
			this.UseAttackTimeEnd = input.ReadValueB32(endianess);
			this.Damage = input.ReadValueF32(endianess);
			this.Impulse.Deserialize(input, endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
		}
	}
}
