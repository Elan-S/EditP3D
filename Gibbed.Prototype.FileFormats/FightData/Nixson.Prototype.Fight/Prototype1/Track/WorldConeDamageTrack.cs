using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.WorldConeDamage)]
	public class WorldConeDamageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public Vector Position { get; set; } = new Vector();
		public Vector Direction { get; set; } = new Vector();
		public Vector Up { get; set; } = new Vector();
		public float MinAngle { get; set; }
		public float MaxAngle { get; set; }
		public float StartDistance { get; set; }
		public float Length { get; set; }
		public int MinDecals { get; set; }
		public int MaxDecals { get; set; }
		public float MinDecalScale { get; set; }
		public float MaxDecalScale { get; set; }
		public DamageType DamageType { get; set; }
		public AttackType AttackType { get; set; }
		public ulong HitType { get; set; }
		public ulong EffectType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			this.Position.Serialize(output, endianess);
			this.Direction.Serialize(output, endianess);
			this.Up.Serialize(output, endianess);
			output.WriteValueF32(this.MinAngle, endianess);
			output.WriteValueF32(this.MaxAngle, endianess);
			output.WriteValueF32(this.StartDistance, endianess);
			output.WriteValueF32(this.Length, endianess);
			output.WriteValueS32(this.MinDecals, endianess);
			output.WriteValueS32(this.MaxDecals, endianess);
			output.WriteValueF32(this.MinDecalScale, endianess);
			output.WriteValueF32(this.MaxDecalScale, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitType, endianess);
			output.WriteValueU64(this.EffectType, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Position.Deserialize(input, endianess);
			this.Direction.Deserialize(input, endianess);
			this.Up.Deserialize(input, endianess);
			this.MinAngle = input.ReadValueF32(endianess);
			this.MaxAngle = input.ReadValueF32(endianess);
			this.StartDistance = input.ReadValueF32(endianess);
			this.Length = input.ReadValueF32(endianess);
			this.MinDecals = input.ReadValueS32(endianess);
			this.MaxDecals = input.ReadValueS32(endianess);
			this.MinDecalScale = input.ReadValueF32(endianess);
			this.MaxDecalScale = input.ReadValueF32(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitType = input.ReadValueU64(endianess);
			this.EffectType = input.ReadValueU64(endianess);
		}
	}
}
