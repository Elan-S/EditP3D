using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12608375028605216058UL)]
	public class DramaticCameraTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool Parent { get; set; }
		public ulong GrabSlot { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public bool TrackCharacter { get; set; }
		public bool AbortWhenInterrupted { get; set; }
		public float FirstFrameHoldTime { get; set; }
		public float LastFrameHoldTime { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public float NoCameraWeight { get; set; }
		public ulong Animation1 { get; set; }
		public float StartFrame1 { get; set; }
		public float EndFrame1 { get; set; }
		public float Speed1 { get; set; }
		public bool EnableNormal1 { get; set; }
		public bool EnableMirror1 { get; set; }
		public float AnimationWeight1 { get; set; }
		public ulong Animation2 { get; set; }
		public float StartFrame2 { get; set; }
		public float EndFrame2 { get; set; }
		public float Speed2 { get; set; }
		public bool EnableNormal2 { get; set; }
		public bool EnableMirror2 { get; set; }
		public float AnimationWeight2 { get; set; }
		public ulong Animation3 { get; set; }
		public float StartFrame3 { get; set; }
		public float EndFrame3 { get; set; }
		public float Speed3 { get; set; }
		public bool EnableNormal3 { get; set; }
		public bool EnableMirror3 { get; set; }
		public float AnimationWeight3 { get; set; }
		public ulong Animation4 { get; set; }
		public float StartFrame4 { get; set; }
		public float EndFrame4 { get; set; }
		public float Speed4 { get; set; }
		public bool EnableNormal4 { get; set; }
		public bool EnableMirror4 { get; set; }
		public float AnimationWeight4 { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.Parent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueB32(this.TrackCharacter, endianess);
			output.WriteValueB32(this.AbortWhenInterrupted, endianess);
			output.WriteValueF32(this.FirstFrameHoldTime, endianess);
			output.WriteValueF32(this.LastFrameHoldTime, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			output.WriteValueF32(this.NoCameraWeight, endianess);
			output.WriteValueU64(this.Animation1, endianess);
			output.WriteValueF32(this.StartFrame1, endianess);
			output.WriteValueF32(this.EndFrame1, endianess);
			output.WriteValueF32(this.Speed1, endianess);
			output.WriteValueB32(this.EnableNormal1, endianess);
			output.WriteValueB32(this.EnableMirror1, endianess);
			output.WriteValueF32(this.AnimationWeight1, endianess);
			output.WriteValueU64(this.Animation2, endianess);
			output.WriteValueF32(this.StartFrame2, endianess);
			output.WriteValueF32(this.EndFrame2, endianess);
			output.WriteValueF32(this.Speed2, endianess);
			output.WriteValueB32(this.EnableNormal2, endianess);
			output.WriteValueB32(this.EnableMirror2, endianess);
			output.WriteValueF32(this.AnimationWeight2, endianess);
			output.WriteValueU64(this.Animation3, endianess);
			output.WriteValueF32(this.StartFrame3, endianess);
			output.WriteValueF32(this.EndFrame3, endianess);
			output.WriteValueF32(this.Speed3, endianess);
			output.WriteValueB32(this.EnableNormal3, endianess);
			output.WriteValueB32(this.EnableMirror3, endianess);
			output.WriteValueF32(this.AnimationWeight3, endianess);
			output.WriteValueU64(this.Animation4, endianess);
			output.WriteValueF32(this.StartFrame4, endianess);
			output.WriteValueF32(this.EndFrame4, endianess);
			output.WriteValueF32(this.Speed4, endianess);
			output.WriteValueB32(this.EnableNormal4, endianess);
			output.WriteValueB32(this.EnableMirror4, endianess);
			output.WriteValueF32(this.AnimationWeight4, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Parent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.TrackCharacter = input.ReadValueB32(endianess);
			this.AbortWhenInterrupted = input.ReadValueB32(endianess);
			this.FirstFrameHoldTime = input.ReadValueF32(endianess);
			this.LastFrameHoldTime = input.ReadValueF32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.NoCameraWeight = input.ReadValueF32(endianess);
			this.Animation1 = input.ReadValueU64(endianess);
			this.StartFrame1 = input.ReadValueF32(endianess);
			this.EndFrame1 = input.ReadValueF32(endianess);
			this.Speed1 = input.ReadValueF32(endianess);
			this.EnableNormal1 = input.ReadValueB32(endianess);
			this.EnableMirror1 = input.ReadValueB32(endianess);
			this.AnimationWeight1 = input.ReadValueF32(endianess);
			this.Animation2 = input.ReadValueU64(endianess);
			this.StartFrame2 = input.ReadValueF32(endianess);
			this.EndFrame2 = input.ReadValueF32(endianess);
			this.Speed2 = input.ReadValueF32(endianess);
			this.EnableNormal2 = input.ReadValueB32(endianess);
			this.EnableMirror2 = input.ReadValueB32(endianess);
			this.AnimationWeight2 = input.ReadValueF32(endianess);
			this.Animation3 = input.ReadValueU64(endianess);
			this.StartFrame3 = input.ReadValueF32(endianess);
			this.EndFrame3 = input.ReadValueF32(endianess);
			this.Speed3 = input.ReadValueF32(endianess);
			this.EnableNormal3 = input.ReadValueB32(endianess);
			this.EnableMirror3 = input.ReadValueB32(endianess);
			this.AnimationWeight3 = input.ReadValueF32(endianess);
			this.Animation4 = input.ReadValueU64(endianess);
			this.StartFrame4 = input.ReadValueF32(endianess);
			this.EndFrame4 = input.ReadValueF32(endianess);
			this.Speed4 = input.ReadValueF32(endianess);
			this.EnableNormal4 = input.ReadValueB32(endianess);
			this.EnableMirror4 = input.ReadValueB32(endianess);
			this.AnimationWeight4 = input.ReadValueF32(endianess);
		}
	}
}
