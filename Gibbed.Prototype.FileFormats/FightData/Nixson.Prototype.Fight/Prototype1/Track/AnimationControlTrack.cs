using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(18166719679364600641UL)]
	public class AnimationControlTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float StartFrame { get; set; }
		public float EndFrame { get; set; }
		public float RelativeSpeed { get; set; }
		public AnimationControlTrack.CycleModeType CycleMode { get; set; }
		public int NumCycles { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.StartFrame, endianess);
			output.WriteValueF32(this.EndFrame, endianess);
			output.WriteValueF32(this.RelativeSpeed, endianess);
			BaseProperty.SerializePropertyEnum<AnimationControlTrack.CycleModeType>(output, endianess, this.CycleMode);
			output.WriteValueS32(this.NumCycles, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.StartFrame = input.ReadValueF32(endianess);
			this.EndFrame = input.ReadValueF32(endianess);
			this.RelativeSpeed = input.ReadValueF32(endianess);
			this.CycleMode = BaseProperty.DeserializePropertyEnum<AnimationControlTrack.CycleModeType>(input, endianess);
			this.NumCycles = input.ReadValueS32(endianess);
		}
		public enum CycleModeType : ulong
		{
			AnimationDefault = 3086659247459262221UL,
			Hold = 20324833833942495UL,
			Forward = 8950535410429879495UL,
			Backward = 11506969557286345811UL,
			ForwardBackward = 13343446975489786740UL,
			BackwardForward = 2555556056228720566UL
		}
	}
}
