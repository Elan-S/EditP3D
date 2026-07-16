using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Effect)]
	public class EffectTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong EffectName { get; set; }
		public ulong ParentObjectName { get; set; }
		public EffectTrack.EmitSpaceType EmitSpace { get; set; }
		public ulong EmitJoint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Orientation { get; set; } = new Vector();
		public AttachSpaceType AttachSpace { get; set; }
		public ulong AttachJoint { get; set; }
		public bool AbortWhenInterrupted { get; set; }
		public ulong SlotName { get; set; }
		public float AbortBelowJointScale { get; set; }
		public float FadeTime { get; set; }
		public ulong EffectController { get; set; }
		public EffectTrack.EffectStoreType EffectStore { get; set; }
		public bool HighPriority { get; set; }
		public float Scale { get; set; }
		public bool Collides { get; set; }
		public float Restitution { get; set; }
		public float Friction { get; set; }
		public float RenderOffset { get; set; }
		public bool UsePropScaling { get; set; }
		public ulong CloneName { get; set; }
		public Color CloneTint { get; set; } = new Color();
		public byte[] Biases { get; set; }
		public float BiasesEmissionRate { get; set; }
		public float BiasesEmissionRateAreaScale { get; set; }
		public ScaleFunction BiasesEmissionRateScaleFunction { get; set; }
		public float BiasesLifeSpan { get; set; }
		public float BiasesLifeSpanVariance { get; set; }
		public float BiasesSpeed { get; set; }
		public float BiasesSpeedVariance { get; set; }
		public float BiasesSize1D { get; set; }
		public float BiasesSize1DVariance { get; set; }
		public float BiasesSize1DAreaScale { get; set; }
		public ScaleFunction BiasesSize1DScaleFunction { get; set; }
		public float BiasesSpin1D { get; set; }
		public float BiasesSpin1DVariance { get; set; }
		public float BiasesWeight { get; set; }
		public float BiasesWeightVariance { get; set; }
		public float BiasesAngle { get; set; }
		public float BiasesAngleVariance { get; set; }
		public float BiasesGravity { get; set; }
		public float BiasesDrag { get; set; }
		public float BiasesColourRed { get; set; }
		public float BiasesColourGreen { get; set; }
		public float BiasesColourBlue { get; set; }
		public float BiasesAlpha { get; set; }
		public float BiasesColourOverLifeRed { get; set; }
		public float BiasesColourOverLifeGreen { get; set; }
		public float BiasesColourOverLifeBlue { get; set; }
		public float BiasesAlphaOverLife { get; set; }
		public float BiasesColourVariance { get; set; }
		public float BiasesAlphaVariance { get; set; }
		public float BiasesGeneratorRadius { get; set; }
		public float BiasesGeneratorRadial { get; set; }
		public float BiasesGeneratorHorizSpread { get; set; }
		public float BiasesGeneratorVertSpread { get; set; }
		public float BiasesGeneratorWidth { get; set; }
		public float BiasesGeneratorHeight { get; set; }
		public Vector BiasesMeshGeneratorScale { get; set; } = new Vector();
		public byte[] Overrides { get; set; }
		public float OverridesEmissionRate { get; set; }
		public float OverridesEmissionRatePerMeter { get; set; }
		public float OverridesMaxParticles { get; set; }
		public float OverridesParticleAllocation { get; set; }
		public float OverridesLifeSpan { get; set; }
		public float OverridesLifeSpanVariance { get; set; }
		public float OverridesSpeed { get; set; }
		public float OverridesSpeedVariance { get; set; }
		public float OverridesSize1D { get; set; }
		public float OverridesSize1DVariance { get; set; }
		public float OverridesSpin1D { get; set; }
		public float OverridesSpin1DVariance { get; set; }
		public float OverridesWeight { get; set; }
		public float OverridesWeightVariance { get; set; }
		public float OverridesAngle { get; set; }
		public float OverridesAngleVariance { get; set; }
		public float OverridesGravity { get; set; }
		public float OverridesDrag { get; set; }
		public float OverridesColourRed { get; set; }
		public float OverridesColourGreen { get; set; }
		public float OverridesColourBlue { get; set; }
		public float OverridesAlpha { get; set; }
		public float OverridesColourVariance { get; set; }
		public float OverridesAlphaVariance { get; set; }
		public float OverridesGeneratorWidth { get; set; }
		public float OverridesGeneratorHeight { get; set; }
		public PropertyTrackGroup Lods { get; set; } = new PropertyTrackGroup(PropertyHash.Lods);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.EffectName, endianess);
			output.WriteValueU64(this.ParentObjectName, endianess);
			BaseProperty.SerializePropertyEnum<EffectTrack.EmitSpaceType>(output, endianess, this.EmitSpace);
			output.WriteValueU64(this.EmitJoint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Orientation.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<AttachSpaceType>(output, endianess, this.AttachSpace);
			output.WriteValueU64(this.AttachJoint, endianess);
			output.WriteValueB32(this.AbortWhenInterrupted, endianess);
			output.WriteValueU64(this.SlotName, endianess);
			output.WriteValueF32(this.AbortBelowJointScale, endianess);
			output.WriteValueF32(this.FadeTime, endianess);
			output.WriteValueU64(this.EffectController, endianess);
			BaseProperty.SerializePropertyEnum<EffectTrack.EffectStoreType>(output, endianess, this.EffectStore);
			output.WriteValueB32(this.HighPriority, endianess);
			output.WriteValueF32(this.Scale, endianess);
			output.WriteValueB32(this.Collides, endianess);
			output.WriteValueF32(this.Restitution, endianess);
			output.WriteValueF32(this.Friction, endianess);
			output.WriteValueF32(this.RenderOffset, endianess);
			output.WriteValueB32(this.UsePropScaling, endianess);
			output.WriteValueU64(this.CloneName, endianess);
			this.CloneTint.Serialize(output, endianess);
			output.WriteBytes(this.Biases);
			output.WriteValueF32(this.BiasesEmissionRate, endianess);
			output.WriteValueF32(this.BiasesEmissionRateAreaScale, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.BiasesEmissionRateScaleFunction);
			output.WriteValueF32(this.BiasesLifeSpan, endianess);
			output.WriteValueF32(this.BiasesLifeSpanVariance, endianess);
			output.WriteValueF32(this.BiasesSpeed, endianess);
			output.WriteValueF32(this.BiasesSpeedVariance, endianess);
			output.WriteValueF32(this.BiasesSize1D, endianess);
			output.WriteValueF32(this.BiasesSize1DVariance, endianess);
			output.WriteValueF32(this.BiasesSize1DAreaScale, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.BiasesSize1DScaleFunction);
			output.WriteValueF32(this.BiasesSpin1D, endianess);
			output.WriteValueF32(this.BiasesSpin1DVariance, endianess);
			output.WriteValueF32(this.BiasesWeight, endianess);
			output.WriteValueF32(this.BiasesWeightVariance, endianess);
			output.WriteValueF32(this.BiasesAngle, endianess);
			output.WriteValueF32(this.BiasesAngleVariance, endianess);
			output.WriteValueF32(this.BiasesGravity, endianess);
			output.WriteValueF32(this.BiasesDrag, endianess);
			output.WriteValueF32(this.BiasesColourRed, endianess);
			output.WriteValueF32(this.BiasesColourGreen, endianess);
			output.WriteValueF32(this.BiasesColourBlue, endianess);
			output.WriteValueF32(this.BiasesAlpha, endianess);
			output.WriteValueF32(this.BiasesColourOverLifeRed, endianess);
			output.WriteValueF32(this.BiasesColourOverLifeGreen, endianess);
			output.WriteValueF32(this.BiasesColourOverLifeBlue, endianess);
			output.WriteValueF32(this.BiasesAlphaOverLife, endianess);
			output.WriteValueF32(this.BiasesColourVariance, endianess);
			output.WriteValueF32(this.BiasesAlphaVariance, endianess);
			output.WriteValueF32(this.BiasesGeneratorRadius, endianess);
			output.WriteValueF32(this.BiasesGeneratorRadial, endianess);
			output.WriteValueF32(this.BiasesGeneratorHorizSpread, endianess);
			output.WriteValueF32(this.BiasesGeneratorVertSpread, endianess);
			output.WriteValueF32(this.BiasesGeneratorWidth, endianess);
			output.WriteValueF32(this.BiasesGeneratorHeight, endianess);
			this.BiasesMeshGeneratorScale.Serialize(output, endianess);
			output.WriteBytes(this.Overrides);
			output.WriteValueF32(this.OverridesEmissionRate, endianess);
			output.WriteValueF32(this.OverridesEmissionRatePerMeter, endianess);
			output.WriteValueF32(this.OverridesMaxParticles, endianess);
			output.WriteValueF32(this.OverridesParticleAllocation, endianess);
			output.WriteValueF32(this.OverridesLifeSpan, endianess);
			output.WriteValueF32(this.OverridesLifeSpanVariance, endianess);
			output.WriteValueF32(this.OverridesSpeed, endianess);
			output.WriteValueF32(this.OverridesSpeedVariance, endianess);
			output.WriteValueF32(this.OverridesSize1D, endianess);
			output.WriteValueF32(this.OverridesSize1DVariance, endianess);
			output.WriteValueF32(this.OverridesSpin1D, endianess);
			output.WriteValueF32(this.OverridesSpin1DVariance, endianess);
			output.WriteValueF32(this.OverridesWeight, endianess);
			output.WriteValueF32(this.OverridesWeightVariance, endianess);
			output.WriteValueF32(this.OverridesAngle, endianess);
			output.WriteValueF32(this.OverridesAngleVariance, endianess);
			output.WriteValueF32(this.OverridesGravity, endianess);
			output.WriteValueF32(this.OverridesDrag, endianess);
			output.WriteValueF32(this.OverridesColourRed, endianess);
			output.WriteValueF32(this.OverridesColourGreen, endianess);
			output.WriteValueF32(this.OverridesColourBlue, endianess);
			output.WriteValueF32(this.OverridesAlpha, endianess);
			output.WriteValueF32(this.OverridesColourVariance, endianess);
			output.WriteValueF32(this.OverridesAlphaVariance, endianess);
			output.WriteValueF32(this.OverridesGeneratorWidth, endianess);
			output.WriteValueF32(this.OverridesGeneratorHeight, endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Lods);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.EffectName = input.ReadValueU64(endianess);
			this.ParentObjectName = input.ReadValueU64(endianess);
			this.EmitSpace = BaseProperty.DeserializePropertyEnum<EffectTrack.EmitSpaceType>(input, endianess);
			this.EmitJoint = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.Orientation = new Vector(input, endianess);
			this.AttachSpace = BaseProperty.DeserializePropertyEnum<AttachSpaceType>(input, endianess);
			this.AttachJoint = input.ReadValueU64(endianess);
			this.AbortWhenInterrupted = input.ReadValueB32(endianess);
			this.SlotName = input.ReadValueU64(endianess);
			this.AbortBelowJointScale = input.ReadValueF32(endianess);
			this.FadeTime = input.ReadValueF32(endianess);
			this.EffectController = input.ReadValueU64(endianess);
			this.EffectStore = BaseProperty.DeserializePropertyEnum<EffectTrack.EffectStoreType>(input, endianess);
			this.HighPriority = input.ReadValueB32(endianess);
			this.Scale = input.ReadValueF32(endianess);
			this.Collides = input.ReadValueB32(endianess);
			this.Restitution = input.ReadValueF32(endianess);
			this.Friction = input.ReadValueF32(endianess);
			this.RenderOffset = input.ReadValueF32(endianess);
			this.UsePropScaling = input.ReadValueB32(endianess);
			this.CloneName = input.ReadValueU64(endianess);
			this.CloneTint = new Color(input, endianess);
			this.Biases = input.ReadBytes(4);
			this.BiasesEmissionRate = input.ReadValueF32(endianess);
			this.BiasesEmissionRateAreaScale = input.ReadValueF32(endianess);
			this.BiasesEmissionRateScaleFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
			this.BiasesLifeSpan = input.ReadValueF32(endianess);
			this.BiasesLifeSpanVariance = input.ReadValueF32(endianess);
			this.BiasesSpeed = input.ReadValueF32(endianess);
			this.BiasesSpeedVariance = input.ReadValueF32(endianess);
			this.BiasesSize1D = input.ReadValueF32(endianess);
			this.BiasesSize1DVariance = input.ReadValueF32(endianess);
			this.BiasesSize1DAreaScale = input.ReadValueF32(endianess);
			this.BiasesSize1DScaleFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
			this.BiasesSpin1D = input.ReadValueF32(endianess);
			this.BiasesSpin1DVariance = input.ReadValueF32(endianess);
			this.BiasesWeight = input.ReadValueF32(endianess);
			this.BiasesWeightVariance = input.ReadValueF32(endianess);
			this.BiasesAngle = input.ReadValueF32(endianess);
			this.BiasesAngleVariance = input.ReadValueF32(endianess);
			this.BiasesGravity = input.ReadValueF32(endianess);
			this.BiasesDrag = input.ReadValueF32(endianess);
			this.BiasesColourRed = input.ReadValueF32(endianess);
			this.BiasesColourGreen = input.ReadValueF32(endianess);
			this.BiasesColourBlue = input.ReadValueF32(endianess);
			this.BiasesAlpha = input.ReadValueF32(endianess);
			this.BiasesColourOverLifeRed = input.ReadValueF32(endianess);
			this.BiasesColourOverLifeGreen = input.ReadValueF32(endianess);
			this.BiasesColourOverLifeBlue = input.ReadValueF32(endianess);
			this.BiasesAlphaOverLife = input.ReadValueF32(endianess);
			this.BiasesColourVariance = input.ReadValueF32(endianess);
			this.BiasesAlphaVariance = input.ReadValueF32(endianess);
			this.BiasesGeneratorRadius = input.ReadValueF32(endianess);
			this.BiasesGeneratorRadial = input.ReadValueF32(endianess);
			this.BiasesGeneratorHorizSpread = input.ReadValueF32(endianess);
			this.BiasesGeneratorVertSpread = input.ReadValueF32(endianess);
			this.BiasesGeneratorWidth = input.ReadValueF32(endianess);
			this.BiasesGeneratorHeight = input.ReadValueF32(endianess);
			this.BiasesMeshGeneratorScale = new Vector(input, endianess);
			this.Overrides = input.ReadBytes(4);
			this.OverridesEmissionRate = input.ReadValueF32(endianess);
			this.OverridesEmissionRatePerMeter = input.ReadValueF32(endianess);
			this.OverridesMaxParticles = input.ReadValueF32(endianess);
			this.OverridesParticleAllocation = input.ReadValueF32(endianess);
			this.OverridesLifeSpan = input.ReadValueF32(endianess);
			this.OverridesLifeSpanVariance = input.ReadValueF32(endianess);
			this.OverridesSpeed = input.ReadValueF32(endianess);
			this.OverridesSpeedVariance = input.ReadValueF32(endianess);
			this.OverridesSize1D = input.ReadValueF32(endianess);
			this.OverridesSize1DVariance = input.ReadValueF32(endianess);
			this.OverridesSpin1D = input.ReadValueF32(endianess);
			this.OverridesSpin1DVariance = input.ReadValueF32(endianess);
			this.OverridesWeight = input.ReadValueF32(endianess);
			this.OverridesWeightVariance = input.ReadValueF32(endianess);
			this.OverridesAngle = input.ReadValueF32(endianess);
			this.OverridesAngleVariance = input.ReadValueF32(endianess);
			this.OverridesGravity = input.ReadValueF32(endianess);
			this.OverridesDrag = input.ReadValueF32(endianess);
			this.OverridesColourRed = input.ReadValueF32(endianess);
			this.OverridesColourGreen = input.ReadValueF32(endianess);
			this.OverridesColourBlue = input.ReadValueF32(endianess);
			this.OverridesAlpha = input.ReadValueF32(endianess);
			this.OverridesColourVariance = input.ReadValueF32(endianess);
			this.OverridesAlphaVariance = input.ReadValueF32(endianess);
			this.OverridesGeneratorWidth = input.ReadValueF32(endianess);
			this.OverridesGeneratorHeight = input.ReadValueF32(endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Lods = BaseProperty.DeserializeTrackProperty(PrototypeGame.P1, input, endianess, PropertyHash.Lods);
		}
		public enum EmitSpaceType : ulong
		{
			WorldSpace = 5062814898204701870UL,
			GameObjectSpace = 15800074974379949085UL,
			GameObjectGlobalUpSpace = 17097909269456236103UL,
			JointSpace = 17857802742059722934UL,
			CollisionSpace = 15089097238419252282UL,
			SurfaceSpace = 12523631433317549063UL
		}
		public enum EffectStoreType : ulong
		{
			General = 1312132684818588012UL,
			MainCharacter = 4221446839816560754UL,
			Character = 11883458559045687845UL,
			Ambient = 2975268716008832216UL,
			Persistent = 9976967587781602699UL,
			Explosion = 6119690315119812053UL,
			Squib = 5872959678684774360UL,
			Tracer = 2105929059253389387UL,
			Bloodtox = 18124356316935178433UL,
			Gameplay = 13278221393172672664UL,
			Devastator = 16626996011609549067UL,
			Collectible = 6733211562610446494UL
		}
	}
}
