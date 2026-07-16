using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11862873709558754054UL)]
	public class AnimationChargeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Animation { get; set; }
		public AnimationChargeTrack.AnimationChargeType Type { get; set; }
		public float StartFrame { get; set; }
		public float EndFrame { get; set; }
		public bool Reverse { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Animation, endianess);
			BaseProperty.SerializePropertyEnum<AnimationChargeTrack.AnimationChargeType>(output, endianess, this.Type);
			output.WriteValueF32(this.StartFrame, endianess);
			output.WriteValueF32(this.EndFrame, endianess);
			output.WriteValueB32(this.Reverse, endianess);
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
			this.Animation = input.ReadValueU64(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<AnimationChargeTrack.AnimationChargeType>(input, endianess);
			this.StartFrame = input.ReadValueF32(endianess);
			this.EndFrame = input.ReadValueF32(endianess);
			this.Reverse = input.ReadValueB32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		public enum AnimationChargeType : ulong
		{
			Invalid = 4122290943349627157UL,
			Jump = 20889331387467136UL,
			Attack = 17648781240126830036UL,
			Throw = 5973634341500805012UL,
			Dive = 19195657991545670UL,
			Glide = 5015188191654984259UL
		}
	}
}
