using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(17407534597370294795UL)]
	public class WorldDamageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Joint { get; set; }
		public Vector Position { get; set; } = new Vector();
		public Vector Direction { get; set; } = new Vector();
		public bool UseSupportingLimb { get; set; }
		public float Distance { get; set; }
		public DamageType DamageType { get; set; }
		public AttackType AttackType { get; set; }
		public ulong HitType { get; set; }
		public ulong EffectType { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public float EffectScale { get; set; }
		public float PropAreaEmissionScale { get; set; }
		public ScaleFunction PropAreaEmissionScaleFunction { get; set; }
		public float PropAreaSizeScale { get; set; }
		public ScaleFunction PropAreaSizeScaleFunction { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Position.Serialize(output, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueB32(this.UseSupportingLimb, endianess);
			output.WriteValueF32(this.Distance, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitType, endianess);
			output.WriteValueU64(this.EffectType, endianess);
			output.WriteValueF32(this.ScaleX, endianess);
			output.WriteValueF32(this.ScaleY, endianess);
			output.WriteValueF32(this.EffectScale, endianess);
			output.WriteValueF32(this.PropAreaEmissionScale, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.PropAreaEmissionScaleFunction);
			output.WriteValueF32(this.PropAreaSizeScale, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.PropAreaSizeScaleFunction);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Position = new Vector(input, endianess);
			this.Direction = new Vector(input, endianess);
			this.UseSupportingLimb = input.ReadValueB32(endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitType = input.ReadValueU64(endianess);
			this.EffectType = input.ReadValueU64(endianess);
			this.ScaleX = input.ReadValueF32(endianess);
			this.ScaleY = input.ReadValueF32(endianess);
			this.EffectScale = input.ReadValueF32(endianess);
			this.PropAreaEmissionScale = input.ReadValueF32(endianess);
			this.PropAreaEmissionScaleFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
			this.PropAreaSizeScale = input.ReadValueF32(endianess);
			this.PropAreaSizeScaleFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
		}
	}
}
