using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(9903731980652888960UL)]
	public class SpawnObjectTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong TemplateName { get; set; }
		public ulong ObjectName { get; set; }
		public bool AddSuffix { get; set; }
		public string InitialState { get; set; }
		public ulong SpecificPlaybackset { get; set; }
		public SpaceType SpawnSpace { get; set; }
		public ulong SpawnJoint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Orientation { get; set; } = new Vector();
		public bool AttachToParent { get; set; }
		public bool DetachAfterSpawn { get; set; }
		public bool AICharacter { get; set; }
		public ulong AutomaticTag { get; set; }
		public ulong GrabSlot { get; set; }
		public bool CopyPose { get; set; }
		public bool InheritSpawnerVelocity { get; set; }
		public Vector LinearVelocity { get; set; } = new Vector();
		public Vector AngularVelocity { get; set; } = new Vector();
		public float CollideWithSpawnerDelay { get; set; }
		public bool SpawnSleeping { get; set; }
		public bool ManageLifetime { get; set; }
		public bool ManageLifetimeThrownProp { get; set; }
		public bool InheritSpawnerTarget { get; set; }
		public ChargeType InheritSpawnerCharge { get; set; }
		public bool UseParentScale { get; set; }
		public bool SetScale { get; set; }
		public Vector Scale { get; set; } = new Vector();
		public float Probability { get; set; }
		public bool UseSpawnerAsDamageOriginator { get; set; }
		public bool DamageOriginatorFromSpawner { get; set; }
		public bool TransferTargetLocks { get; set; }
		public bool SendMessageToScenarioTree { get; set; }
		public bool ApplyColourTint { get; set; }
		public bool InheritPackage { get; set; }
		public Color TintColour { get; set; } = new Color();
		public bool CacheDamageOrigionatorAsCollider { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.TemplateName, endianess);
			output.WriteValueU64(this.ObjectName, endianess);
			output.WriteValueB32(this.AddSuffix, endianess);
			output.WriteStringAlignedU32(this.InitialState, endianess);
			output.WriteValueU64(this.SpecificPlaybackset, endianess);
			BaseProperty.SerializePropertyEnum<SpaceType>(output, endianess, this.SpawnSpace);
			output.WriteValueU64(this.SpawnJoint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Orientation.Serialize(output, endianess);
			output.WriteValueB32(this.AttachToParent, endianess);
			output.WriteValueB32(this.DetachAfterSpawn, endianess);
			output.WriteValueB32(this.AICharacter, endianess);
			output.WriteValueU64(this.AutomaticTag, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueB32(this.CopyPose, endianess);
			output.WriteValueB32(this.InheritSpawnerVelocity, endianess);
			this.LinearVelocity.Serialize(output, endianess);
			this.AngularVelocity.Serialize(output, endianess);
			output.WriteValueF32(this.CollideWithSpawnerDelay, endianess);
			output.WriteValueB32(this.SpawnSleeping, endianess);
			output.WriteValueB32(this.ManageLifetime, endianess);
			output.WriteValueB32(this.ManageLifetimeThrownProp, endianess);
			output.WriteValueB32(this.InheritSpawnerTarget, endianess);
			BaseProperty.SerializePropertyEnum<ChargeType>(output, endianess, this.InheritSpawnerCharge);
			output.WriteValueB32(this.UseParentScale, endianess);
			output.WriteValueB32(this.SetScale, endianess);
			this.Scale.Serialize(output, endianess);
			output.WriteValueF32(this.Probability, endianess);
			output.WriteValueB32(this.UseSpawnerAsDamageOriginator, endianess);
			output.WriteValueB32(this.DamageOriginatorFromSpawner, endianess);
			output.WriteValueB32(this.TransferTargetLocks, endianess);
			output.WriteValueB32(this.SendMessageToScenarioTree, endianess);
			output.WriteValueB32(this.ApplyColourTint, endianess);
			output.WriteValueB32(this.InheritPackage, endianess);
			this.TintColour.Serialize(output, endianess);
			output.WriteValueB32(this.CacheDamageOrigionatorAsCollider, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TemplateName = input.ReadValueU64(endianess);
			this.ObjectName = input.ReadValueU64(endianess);
			this.AddSuffix = input.ReadValueB32(endianess);
			this.InitialState = input.ReadStringAlignedU32(endianess);
			this.SpecificPlaybackset = input.ReadValueU64(endianess);
			this.SpawnSpace = BaseProperty.DeserializePropertyEnum<SpaceType>(input, endianess);
			this.SpawnJoint = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.Orientation = new Vector(input, endianess);
			this.AttachToParent = input.ReadValueB32(endianess);
			this.DetachAfterSpawn = input.ReadValueB32(endianess);
			this.AICharacter = input.ReadValueB32(endianess);
			this.AutomaticTag = input.ReadValueU64(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.CopyPose = input.ReadValueB32(endianess);
			this.InheritSpawnerVelocity = input.ReadValueB32(endianess);
			this.LinearVelocity = new Vector(input, endianess);
			this.AngularVelocity = new Vector(input, endianess);
			this.CollideWithSpawnerDelay = input.ReadValueF32(endianess);
			this.SpawnSleeping = input.ReadValueB32(endianess);
			this.ManageLifetime = input.ReadValueB32(endianess);
			this.ManageLifetimeThrownProp = input.ReadValueB32(endianess);
			this.InheritSpawnerTarget = input.ReadValueB32(endianess);
			this.InheritSpawnerCharge = BaseProperty.DeserializePropertyEnum<ChargeType>(input, endianess);
			this.UseParentScale = input.ReadValueB32(endianess);
			this.SetScale = input.ReadValueB32(endianess);
			this.Scale = new Vector(input, endianess);
			this.Probability = input.ReadValueF32(endianess);
			this.UseSpawnerAsDamageOriginator = input.ReadValueB32(endianess);
			this.DamageOriginatorFromSpawner = input.ReadValueB32(endianess);
			this.TransferTargetLocks = input.ReadValueB32(endianess);
			this.SendMessageToScenarioTree = input.ReadValueB32(endianess);
			this.ApplyColourTint = input.ReadValueB32(endianess);
			this.InheritPackage = input.ReadValueB32(endianess);
			this.TintColour = new Color(input, endianess);
			this.CacheDamageOrigionatorAsCollider = input.ReadValueB32(endianess);
		}
	}
}
