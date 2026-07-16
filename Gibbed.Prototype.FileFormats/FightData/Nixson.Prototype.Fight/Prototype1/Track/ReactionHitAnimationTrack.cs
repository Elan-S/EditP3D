using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17625122889665648638UL)]
	public class ReactionHitAnimationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeScale { get; set; }
		public float TimeEndMin { get; set; }
		public ulong Animation { get; set; }
		public float Speed { get; set; }
		public float SpeedMax { get; set; }
		public float InitFrame { get; set; }
		public float RandomFrame { get; set; }
		public float StartFrame { get; set; }
		public float EndFrame { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeScale, endianess);
			output.WriteValueF32(this.TimeEndMin, endianess);
			output.WriteValueU64(this.Animation, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueF32(this.SpeedMax, endianess);
			output.WriteValueF32(this.InitFrame, endianess);
			output.WriteValueF32(this.RandomFrame, endianess);
			output.WriteValueF32(this.StartFrame, endianess);
			output.WriteValueF32(this.EndFrame, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueU64(this.Partition, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeScale = input.ReadValueF32(endianess);
			this.TimeEndMin = input.ReadValueF32(endianess);
			this.Animation = input.ReadValueU64(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.SpeedMax = input.ReadValueF32(endianess);
			this.InitFrame = input.ReadValueF32(endianess);
			this.RandomFrame = input.ReadValueF32(endianess);
			this.StartFrame = input.ReadValueF32(endianess);
			this.EndFrame = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
