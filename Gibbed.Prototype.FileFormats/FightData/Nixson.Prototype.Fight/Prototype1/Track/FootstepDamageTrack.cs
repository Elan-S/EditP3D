using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12028693053772567405UL)]
	public class FootstepDamageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Phase { get; set; }
		public ulong JointName { get; set; }
		public Vector JointOffset { get; set; } = new Vector();
		public DamageType DamageType { get; set; }
		public AttackType AttackType { get; set; }
		public ulong HitType { get; set; }
		public ulong EffectType { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Phase, endianess);
			output.WriteValueU64(this.JointName, endianess);
			this.JointOffset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitType, endianess);
			output.WriteValueU64(this.EffectType, endianess);
			output.WriteValueF32(this.ScaleX, endianess);
			output.WriteValueF32(this.ScaleY, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Phase = input.ReadValueF32(endianess);
			this.JointName = input.ReadValueU64(endianess);
			this.JointOffset = new Vector(input, endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitType = input.ReadValueU64(endianess);
			this.EffectType = input.ReadValueU64(endianess);
			this.ScaleX = input.ReadValueF32(endianess);
			this.ScaleY = input.ReadValueF32(endianess);
		}
	}
}
