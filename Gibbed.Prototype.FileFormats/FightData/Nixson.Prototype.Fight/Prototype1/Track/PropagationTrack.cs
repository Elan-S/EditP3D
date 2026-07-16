using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Propagation)]
	public class PropagationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float MaxDeflection { get; set; }
		public bool EnableDirection { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public float Distance { get; set; }
		public float DamagePercentage { get; set; }
		public DamageType DamageType { get; set; }
		public AttackType AttackType { get; set; }
		public ulong HitType { get; set; }
		public ulong EffectType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.MaxDeflection, endianess);
			output.WriteValueB32(this.EnableDirection, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueF32(this.Distance, endianess);
			output.WriteValueF32(this.DamagePercentage, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitType, endianess);
			output.WriteValueU64(this.EffectType, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.MaxDeflection = input.ReadValueF32(endianess);
			this.EnableDirection = input.ReadValueB32(endianess);
			this.Direction.Deserialize(input, endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.DamagePercentage = input.ReadValueF32(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitType = input.ReadValueU64(endianess);
			this.EffectType = input.ReadValueU64(endianess);
		}
	}
}
