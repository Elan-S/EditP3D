using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Hit)]
	public class CollisionHitTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public bool UseShapecast { get; set; }
		public int CollisionCountMax { get; set; }
		public float TimeBetweenHits { get; set; }
		public ulong jointName { get; set; }
		public ulong jointEnd { get; set; }
		public float OffsetX { get; set; }
		public float OffsetY { get; set; }
		public float OffsetZ { get; set; }
		public float Radius { get; set; }
		public float ArcOffset { get; set; }
		public float ArcRange { get; set; }
		public Collidables CollideWith { get; set; }
		public float DamageMin { get; set; }
		public float DamageMax { get; set; }
		public bool UseTransformationDamageMultiplier { get; set; }
		public bool UseWeaponDamageMultiplier { get; set; }
		public float FriendlyFire { get; set; }
		public AttackType AttackType { get; set; }
		public ulong HitTypeName { get; set; }
		public DamageType DamageType { get; set; }
		public ulong EffectTypeName { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public float EffectScale { get; set; }
		public float PropAreaEmissionScale { get; set; }
		public ScaleFunction PropAreaEmissionScaleFunction { get; set; }
		public float PropAreaSizeScale { get; set; }
		public ScaleFunction propAreaSizeScaleFunction { get; set; }
		public float ImpulseMinX { get; set; }
		public float ImpulseMinY { get; set; }
		public float ImpulseMinZ { get; set; }
		public float ImpulseMaxX { get; set; }
		public float ImpulseMaxY { get; set; }
		public float ImpulseMaxZ { get; set; }
		public float ImpulseRandomPlusMinus { get; set; }
		public bool UseMotion { get; set; }
		public float CollisionDirectionX { get; set; }
		public float CollisionDirectionY { get; set; }
		public float CollisionDirectionZ { get; set; }
		public float DeformDirStr { get; set; }
		public float DeformStr { get; set; }
		public ulong CollisionFlagName { get; set; }
		public BranchReference GiverBranchRef { get; set; } = new BranchReference();
		public BranchReference ReceiverBranchRef { get; set; } = new BranchReference();
		public bool UseDamageOriginator { get; set; }
		public bool SendAlert { get; set; }
		public bool SendOnlyToAIManager { get; set; }
		public bool UseOriginatorToValidate { get; set; }
		public bool IsThrown { get; set; }
		public float MomentumDamageVelocity { get; set; }
		public float MomentumDamageMax { get; set; }
		public float TravelVelocity { get; set; }
		public CollisionHitTrack.TravelMode TravelTowardsTarget { get; set; }
		public bool IgnorePowerTarget { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			output.WriteValueB32(this.UseShapecast, endianess);
			output.WriteValueS32(this.CollisionCountMax, endianess);
			output.WriteValueF32(this.TimeBetweenHits, endianess);
			output.WriteValueU64(this.jointName, endianess);
			output.WriteValueU64(this.jointEnd, endianess);
			output.WriteValueF32(this.OffsetX, endianess);
			output.WriteValueF32(this.OffsetY, endianess);
			output.WriteValueF32(this.OffsetZ, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.ArcOffset, endianess);
			output.WriteValueF32(this.ArcRange, endianess);
			BaseProperty.SerializePropertyBitfield<Collidables>(output, endianess, this.CollideWith);
			output.WriteValueF32(this.DamageMin, endianess);
			output.WriteValueF32(this.DamageMax, endianess);
			output.WriteValueB32(this.UseTransformationDamageMultiplier, endianess);
			output.WriteValueB32(this.UseWeaponDamageMultiplier, endianess);
			output.WriteValueF32(this.FriendlyFire, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitTypeName, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			output.WriteValueU64(this.EffectTypeName, endianess);
			output.WriteValueF32(this.ScaleX, endianess);
			output.WriteValueF32(this.ScaleY, endianess);
			output.WriteValueF32(this.EffectScale, endianess);
			output.WriteValueF32(this.PropAreaEmissionScale, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.PropAreaEmissionScaleFunction);
			output.WriteValueF32(this.PropAreaSizeScale, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.propAreaSizeScaleFunction);
			output.WriteValueF32(this.ImpulseMinX, endianess);
			output.WriteValueF32(this.ImpulseMinY, endianess);
			output.WriteValueF32(this.ImpulseMinZ, endianess);
			output.WriteValueF32(this.ImpulseMaxX, endianess);
			output.WriteValueF32(this.ImpulseMaxY, endianess);
			output.WriteValueF32(this.ImpulseMaxZ, endianess);
			output.WriteValueF32(this.ImpulseRandomPlusMinus, endianess);
			output.WriteValueB32(this.UseMotion, endianess);
			output.WriteValueF32(this.CollisionDirectionX, endianess);
			output.WriteValueF32(this.CollisionDirectionY, endianess);
			output.WriteValueF32(this.CollisionDirectionZ, endianess);
			output.WriteValueF32(this.DeformDirStr, endianess);
			output.WriteValueF32(this.DeformStr, endianess);
			output.WriteValueU64(this.CollisionFlagName, endianess);
			this.GiverBranchRef.Serialize(output, endianess);
			this.ReceiverBranchRef.Serialize(output, endianess);
			output.WriteValueB32(this.UseDamageOriginator, endianess);
			output.WriteValueB32(this.SendAlert, endianess);
			output.WriteValueB32(this.SendOnlyToAIManager, endianess);
			output.WriteValueB32(this.UseOriginatorToValidate, endianess);
			output.WriteValueB32(this.IsThrown, endianess);
			output.WriteValueF32(this.MomentumDamageVelocity, endianess);
			output.WriteValueF32(this.MomentumDamageMax, endianess);
			output.WriteValueF32(this.TravelVelocity, endianess);
			BaseProperty.SerializePropertyEnum<CollisionHitTrack.TravelMode>(output, endianess, this.TravelTowardsTarget);
			output.WriteValueB32(this.IgnorePowerTarget, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
			this.UseShapecast = input.ReadValueB32(endianess);
			this.CollisionCountMax = input.ReadValueS32(endianess);
			this.TimeBetweenHits = input.ReadValueF32(endianess);
			this.jointName = input.ReadValueU64(endianess);
			this.jointEnd = input.ReadValueU64(endianess);
			this.OffsetX = input.ReadValueF32(endianess);
			this.OffsetY = input.ReadValueF32(endianess);
			this.OffsetZ = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.ArcOffset = input.ReadValueF32(endianess);
			this.ArcRange = input.ReadValueF32(endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<Collidables>(input, endianess);
			this.DamageMin = input.ReadValueF32(endianess);
			this.DamageMax = input.ReadValueF32(endianess);
			this.UseTransformationDamageMultiplier = input.ReadValueB32(endianess);
			this.UseWeaponDamageMultiplier = input.ReadValueB32(endianess);
			this.FriendlyFire = input.ReadValueF32(endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitTypeName = input.ReadValueU64(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.EffectTypeName = input.ReadValueU64(endianess);
			this.ScaleX = input.ReadValueF32(endianess);
			this.ScaleY = input.ReadValueF32(endianess);
			this.EffectScale = input.ReadValueF32(endianess);
			this.PropAreaEmissionScale = input.ReadValueF32(endianess);
			this.PropAreaEmissionScaleFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
			this.PropAreaSizeScale = input.ReadValueF32(endianess);
			this.propAreaSizeScaleFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
			this.ImpulseMinX = input.ReadValueF32(endianess);
			this.ImpulseMinY = input.ReadValueF32(endianess);
			this.ImpulseMinZ = input.ReadValueF32(endianess);
			this.ImpulseMaxX = input.ReadValueF32(endianess);
			this.ImpulseMaxY = input.ReadValueF32(endianess);
			this.ImpulseMaxZ = input.ReadValueF32(endianess);
			this.ImpulseRandomPlusMinus = input.ReadValueF32(endianess);
			this.UseMotion = input.ReadValueB32(endianess);
			this.CollisionDirectionX = input.ReadValueF32(endianess);
			this.CollisionDirectionY = input.ReadValueF32(endianess);
			this.CollisionDirectionZ = input.ReadValueF32(endianess);
			this.DeformDirStr = input.ReadValueF32(endianess);
			this.DeformStr = input.ReadValueF32(endianess);
			this.CollisionFlagName = input.ReadValueU64(endianess);
			this.GiverBranchRef = new BranchReference(input, endianess);
			this.ReceiverBranchRef = new BranchReference(input, endianess);
			this.UseDamageOriginator = input.ReadValueB32(endianess);
			this.SendAlert = input.ReadValueB32(endianess);
			this.SendOnlyToAIManager = input.ReadValueB32(endianess);
			this.UseOriginatorToValidate = input.ReadValueB32(endianess);
			this.IsThrown = input.ReadValueB32(endianess);
			this.MomentumDamageVelocity = input.ReadValueF32(endianess);
			this.MomentumDamageMax = input.ReadValueF32(endianess);
			this.TravelVelocity = input.ReadValueF32(endianess);
			this.TravelTowardsTarget = BaseProperty.DeserializePropertyEnum<CollisionHitTrack.TravelMode>(input, endianess);
			this.IgnorePowerTarget = input.ReadValueB32(endianess);
		}
		public enum TravelMode : ulong
		{
			None = 22018610510307286UL,
			Sphere = 2636713307244341551UL,
			ExtendingCapsule = 10827414619816049493UL
		}
	}
}
