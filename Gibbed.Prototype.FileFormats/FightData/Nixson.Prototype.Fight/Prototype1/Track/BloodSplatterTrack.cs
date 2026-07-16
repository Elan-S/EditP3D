using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14118766043957330453UL)]
	public class BloodSplatterTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Shader { get; set; }
		public float Wait { get; set; }
		public float Duration { get; set; }
		public float FadeDuration { get; set; }
		public float WaitRand { get; set; }
		public float DurationRand { get; set; }
		public float FadeDurationRand { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public float PositionXRand { get; set; }
		public float PositionYRand { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public float ScaleXYRand { get; set; }
		public float ScaleXRand { get; set; }
		public float ScaleYRand { get; set; }
		public float Angle { get; set; }
		public float AngleRand { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Shader, endianess);
			output.WriteValueF32(this.Wait, endianess);
			output.WriteValueF32(this.Duration, endianess);
			output.WriteValueF32(this.FadeDuration, endianess);
			output.WriteValueF32(this.WaitRand, endianess);
			output.WriteValueF32(this.DurationRand, endianess);
			output.WriteValueF32(this.FadeDurationRand, endianess);
			output.WriteValueF32(this.PositionX, endianess);
			output.WriteValueF32(this.PositionY, endianess);
			output.WriteValueF32(this.PositionXRand, endianess);
			output.WriteValueF32(this.PositionYRand, endianess);
			output.WriteValueF32(this.ScaleX, endianess);
			output.WriteValueF32(this.ScaleY, endianess);
			output.WriteValueF32(this.ScaleXYRand, endianess);
			output.WriteValueF32(this.ScaleXRand, endianess);
			output.WriteValueF32(this.ScaleYRand, endianess);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueF32(this.AngleRand, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Shader = input.ReadValueU64(endianess);
			this.Wait = input.ReadValueF32(endianess);
			this.Duration = input.ReadValueF32(endianess);
			this.FadeDuration = input.ReadValueF32(endianess);
			this.WaitRand = input.ReadValueF32(endianess);
			this.DurationRand = input.ReadValueF32(endianess);
			this.FadeDurationRand = input.ReadValueF32(endianess);
			this.PositionX = input.ReadValueF32(endianess);
			this.PositionY = input.ReadValueF32(endianess);
			this.PositionXRand = input.ReadValueF32(endianess);
			this.PositionYRand = input.ReadValueF32(endianess);
			this.ScaleX = input.ReadValueF32(endianess);
			this.ScaleY = input.ReadValueF32(endianess);
			this.ScaleXYRand = input.ReadValueF32(endianess);
			this.ScaleXRand = input.ReadValueF32(endianess);
			this.ScaleYRand = input.ReadValueF32(endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.AngleRand = input.ReadValueF32(endianess);
		}
	}
}
