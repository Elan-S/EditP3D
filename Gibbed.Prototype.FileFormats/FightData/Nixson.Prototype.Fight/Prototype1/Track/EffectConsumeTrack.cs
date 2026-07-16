using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.EffectConsume)]
	public class EffectConsumeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Shader { get; set; }
		public float SpreadRate { get; set; }
		public float GrowthThreshold { get; set; }
		public EffectConsumeTrack.ConsumeModeType ConsumeMode { get; set; }
		public float TimeReverse { get; set; }
		public bool UseTimeReveal { get; set; }
		public float TimeReveal { get; set; }
		public EffectConsumeTrack.RevealModeType RevealMode { get; set; }
		public ulong StartJoint1 { get; set; }
		public Vector StartOffset1 { get; set; } = new Vector();
		public bool UseSecondStartPoint { get; set; }
		public ulong StartJoint2 { get; set; }
		public Vector StartOffset2 { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Shader, endianess);
			output.WriteValueF32(this.SpreadRate, endianess);
			output.WriteValueF32(this.GrowthThreshold, endianess);
			BaseProperty.SerializePropertyEnum<EffectConsumeTrack.ConsumeModeType>(output, endianess, this.ConsumeMode);
			output.WriteValueF32(this.TimeReverse, endianess);
			output.WriteValueB32(this.UseTimeReveal, endianess);
			output.WriteValueF32(this.TimeReveal, endianess);
			BaseProperty.SerializePropertyEnum<EffectConsumeTrack.RevealModeType>(output, endianess, this.RevealMode);
			output.WriteValueU64(this.StartJoint1, endianess);
			this.StartOffset1.Serialize(output, endianess);
			output.WriteValueB32(this.UseSecondStartPoint, endianess);
			output.WriteValueU64(this.StartJoint2, endianess);
			this.StartOffset2.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Shader = input.ReadValueU64(endianess);
			this.SpreadRate = input.ReadValueF32(endianess);
			this.GrowthThreshold = input.ReadValueF32(endianess);
			this.ConsumeMode = BaseProperty.DeserializePropertyEnum<EffectConsumeTrack.ConsumeModeType>(input, endianess);
			this.TimeReverse = input.ReadValueF32(endianess);
			this.UseTimeReveal = input.ReadValueB32(endianess);
			this.TimeReveal = input.ReadValueF32(endianess);
			this.RevealMode = BaseProperty.DeserializePropertyEnum<EffectConsumeTrack.RevealModeType>(input, endianess);
			this.StartJoint1 = input.ReadValueU64(endianess);
			this.StartOffset1 = new Vector(input, endianess);
			this.UseSecondStartPoint = input.ReadValueB32(endianess);
			this.StartJoint2 = input.ReadValueU64(endianess);
			this.StartOffset2 = new Vector(input, endianess);
		}
		public enum ConsumeModeType : ulong
		{
			AnimationDefault = 3086659247459262221UL,
			Hold = 20324833833942495UL,
			Forward = 8950535410429879495UL,
			Backward = 11506969557286345811UL,
			ForwardBackward = 13343446975489786740UL,
			BackwardForward = 2555556056228720566UL
		}
		public enum RevealModeType : ulong
		{
			None = 22018610510307286UL,
			Solid = 5865620095936752093UL,
			FadeOut = 8691507612549003382UL,
			FadeIn = 975912642622236387UL
		}
	}
}
