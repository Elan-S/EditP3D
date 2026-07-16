using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ExpressionAnimation)]
	public class ExpressionAnimationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Animation { get; set; }
		public float Speed { get; set; }
		public float InitFrame { get; set; }
		public float StartFrame { get; set; }
		public float EndFrame { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Animation, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueF32(this.InitFrame, endianess);
			output.WriteValueF32(this.StartFrame, endianess);
			output.WriteValueF32(this.EndFrame, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Animation = input.ReadValueU64(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.InitFrame = input.ReadValueF32(endianess);
			this.StartFrame = input.ReadValueF32(endianess);
			this.EndFrame = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
		}
	}
}
