using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11861372874423754554UL)]
	public class JumpAnimationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Animation { get; set; }
		public float Gravity { get; set; }
		public float PeakVerticalVelocity { get; set; }
		public float StartFrameClimb { get; set; }
		public float EndFrameClimb { get; set; }
		public float StartFramePeak { get; set; }
		public float EndFramePeak { get; set; }
		public float StartFrameFall { get; set; }
		public float EndFrameFall { get; set; }
		public bool Spew { get; set; }
		public bool HasRootTranslation { get; set; }
		public bool HasRootRotation { get; set; }
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
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.PeakVerticalVelocity, endianess);
			output.WriteValueF32(this.StartFrameClimb, endianess);
			output.WriteValueF32(this.EndFrameClimb, endianess);
			output.WriteValueF32(this.StartFramePeak, endianess);
			output.WriteValueF32(this.EndFramePeak, endianess);
			output.WriteValueF32(this.StartFrameFall, endianess);
			output.WriteValueF32(this.EndFrameFall, endianess);
			output.WriteValueB32(this.Spew, endianess);
			output.WriteValueB32(this.HasRootTranslation, endianess);
			output.WriteValueB32(this.HasRootRotation, endianess);
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
			this.Gravity = input.ReadValueF32(endianess);
			this.PeakVerticalVelocity = input.ReadValueF32(endianess);
			this.StartFrameClimb = input.ReadValueF32(endianess);
			this.EndFrameClimb = input.ReadValueF32(endianess);
			this.StartFramePeak = input.ReadValueF32(endianess);
			this.EndFramePeak = input.ReadValueF32(endianess);
			this.StartFrameFall = input.ReadValueF32(endianess);
			this.EndFrameFall = input.ReadValueF32(endianess);
			this.Spew = input.ReadValueB32(endianess);
			this.HasRootTranslation = input.ReadValueB32(endianess);
			this.HasRootRotation = input.ReadValueB32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
