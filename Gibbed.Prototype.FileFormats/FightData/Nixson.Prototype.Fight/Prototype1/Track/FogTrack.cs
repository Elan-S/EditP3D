using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Fog)]
	public class FogTrack : P1Track
	{
		public float Wait { get; set; }
		public float FadeIn { get; set; }
		public float Duration { get; set; }
		public float FadeOut { get; set; }
		public ScaleFunction FadingFunction { get; set; }
		public bool Enable { get; set; }
		public ulong EffectController { get; set; }
		public AttachSpaceType EmitSpace { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public AttachSpaceType AttachSpace { get; set; }
		public Vector FogAreaSize { get; set; } = new Vector();
		public float GeneratorWidthBias { get; set; }
		public float GeneratorHeightBias { get; set; }
		public float FogThickness { get; set; }
		public Color DarkTint { get; set; } = new Color();
		public Color LightTint { get; set; } = new Color();
		public float FogHorizTiling { get; set; }
		public float FogVertTiling { get; set; }
		public bool RandomMoveDir { get; set; }
		public Vector FogMoveDir { get; set; } = new Vector();
		public float Speed { get; set; }
		public float FogFalloff { get; set; }
		public float FalloffModifier { get; set; }
		public float FogVariation { get; set; }
		public float NoiseStrength { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.Wait, endianess);
			output.WriteValueF32(this.FadeIn, endianess);
			output.WriteValueF32(this.Duration, endianess);
			output.WriteValueF32(this.FadeOut, endianess);
			BaseProperty.SerializePropertyEnum<ScaleFunction>(output, endianess, this.FadingFunction);
			output.WriteValueB32(this.Enable, endianess);
			output.WriteValueU64(this.EffectController, endianess);
			BaseProperty.SerializePropertyEnum<AttachSpaceType>(output, endianess, this.EmitSpace);
			this.Offset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<AttachSpaceType>(output, endianess, this.AttachSpace);
			this.FogAreaSize.Serialize(output, endianess);
			output.WriteValueF32(this.GeneratorWidthBias, endianess);
			output.WriteValueF32(this.GeneratorHeightBias, endianess);
			output.WriteValueF32(this.FogThickness, endianess);
			this.DarkTint.Serialize(output, endianess);
			this.LightTint.Serialize(output, endianess);
			output.WriteValueF32(this.FogHorizTiling, endianess);
			output.WriteValueF32(this.FogVertTiling, endianess);
			output.WriteValueB32(this.RandomMoveDir, endianess);
			this.FogMoveDir.Serialize(output, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueF32(this.FogFalloff, endianess);
			output.WriteValueF32(this.FalloffModifier, endianess);
			output.WriteValueF32(this.FogVariation, endianess);
			output.WriteValueF32(this.NoiseStrength, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Wait = input.ReadValueF32(endianess);
			this.FadeIn = input.ReadValueF32(endianess);
			this.Duration = input.ReadValueF32(endianess);
			this.FadeOut = input.ReadValueF32(endianess);
			this.FadingFunction = BaseProperty.DeserializePropertyEnum<ScaleFunction>(input, endianess);
			this.Enable = input.ReadValueB32(endianess);
			this.EffectController = input.ReadValueU64(endianess);
			this.EmitSpace = BaseProperty.DeserializePropertyEnum<AttachSpaceType>(input, endianess);
			this.Offset.Deserialize(input, endianess);
			this.AttachSpace = BaseProperty.DeserializePropertyEnum<AttachSpaceType>(input, endianess);
			this.FogAreaSize.Deserialize(input, endianess);
			this.GeneratorWidthBias = input.ReadValueF32(endianess);
			this.GeneratorHeightBias = input.ReadValueF32(endianess);
			this.FogThickness = input.ReadValueF32(endianess);
			this.DarkTint.Deserialize(input, endianess);
			this.LightTint.Deserialize(input, endianess);
			this.FogHorizTiling = input.ReadValueF32(endianess);
			this.FogVertTiling = input.ReadValueF32(endianess);
			this.RandomMoveDir = input.ReadValueB32(endianess);
			this.FogMoveDir.Deserialize(input, endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.FogFalloff = input.ReadValueF32(endianess);
			this.FalloffModifier = input.ReadValueF32(endianess);
			this.FogVariation = input.ReadValueF32(endianess);
			this.NoiseStrength = input.ReadValueF32(endianess);
		}
	}
}
