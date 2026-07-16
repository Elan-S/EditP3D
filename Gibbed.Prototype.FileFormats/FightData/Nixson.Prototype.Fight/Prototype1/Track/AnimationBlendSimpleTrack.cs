using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10025496823848995837UL)]
	public class AnimationBlendSimpleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong WeightVariable { get; set; }
		public float Speed { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public ulong AnimA { get; set; }
		public float AnimASyncFrame { get; set; }
		public float AnimAStartFrame { get; set; }
		public float AnimAEndFrame { get; set; }
		public float AnimASpeed { get; set; }
		public ulong AnimB { get; set; }
		public float AnimBSyncFrame { get; set; }
		public float AnimBStartFrame { get; set; }
		public float AnimBEndFrame { get; set; }
		public float AnimBSpeed { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.WeightVariable, endianess);
			output.WriteValueF32(this.Speed, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueU64(this.AnimA, endianess);
			output.WriteValueF32(this.AnimASyncFrame, endianess);
			output.WriteValueF32(this.AnimAStartFrame, endianess);
			output.WriteValueF32(this.AnimAEndFrame, endianess);
			output.WriteValueF32(this.AnimASpeed, endianess);
			output.WriteValueU64(this.AnimB, endianess);
			output.WriteValueF32(this.AnimBSyncFrame, endianess);
			output.WriteValueF32(this.AnimBStartFrame, endianess);
			output.WriteValueF32(this.AnimBEndFrame, endianess);
			output.WriteValueF32(this.AnimBSpeed, endianess);
			output.WriteValueU64(this.Partition, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.WeightVariable = input.ReadValueU64(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.AnimA = input.ReadValueU64(endianess);
			this.AnimASyncFrame = input.ReadValueF32(endianess);
			this.AnimAStartFrame = input.ReadValueF32(endianess);
			this.AnimAEndFrame = input.ReadValueF32(endianess);
			this.AnimASpeed = input.ReadValueF32(endianess);
			this.AnimB = input.ReadValueU64(endianess);
			this.AnimBSyncFrame = input.ReadValueF32(endianess);
			this.AnimBStartFrame = input.ReadValueF32(endianess);
			this.AnimBEndFrame = input.ReadValueF32(endianess);
			this.AnimBSpeed = input.ReadValueF32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
