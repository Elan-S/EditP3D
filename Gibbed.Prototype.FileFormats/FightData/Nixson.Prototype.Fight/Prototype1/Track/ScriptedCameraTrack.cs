using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(15069011368341203315UL)]
	public class ScriptedCameraTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool AutoEnd { get; set; }
		public bool Parent { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong Joint { get; set; }
		public float OffsetX { get; set; }
		public float OffsetY { get; set; }
		public float OffsetZ { get; set; }
		public bool TrackObject { get; set; }
		public ulong Animation { get; set; }
		public bool RotateY { get; set; }
		public bool MirrorX { get; set; }
		public float StartFrame { get; set; }
		public float EndFrame { get; set; }
		public float Speed { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public float DofNear { get; set; }
		public float DofFar { get; set; }
		public float DofRange { get; set; }
		public float DofAperture { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.AutoEnd, endianess);
			output.WriteValueB32(this.Parent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueF32(this.OffsetX, endianess);
			output.WriteValueF32(this.OffsetY, endianess);
			output.WriteValueF32(this.OffsetZ, endianess);
			output.WriteValueB32(this.TrackObject, endianess);
			output.WriteValueU64(this.Animation, endianess);
			output.WriteValueB32(this.RotateY, endianess);
			output.WriteValueB32(this.MirrorX, endianess);
			output.WriteValueF32(this.StartFrame, endianess);
			output.WriteValueF32(this.EndFrame, endianess);
			output.WriteValueF32(this.Speed, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueF32(this.DofNear, endianess);
			output.WriteValueF32(this.DofFar, endianess);
			output.WriteValueF32(this.DofRange, endianess);
			output.WriteValueF32(this.DofAperture, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.AutoEnd = input.ReadValueB32(endianess);
			this.Parent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.OffsetX = input.ReadValueF32(endianess);
			this.OffsetY = input.ReadValueF32(endianess);
			this.OffsetZ = input.ReadValueF32(endianess);
			this.TrackObject = input.ReadValueB32(endianess);
			this.Animation = input.ReadValueU64(endianess);
			this.RotateY = input.ReadValueB32(endianess);
			this.MirrorX = input.ReadValueB32(endianess);
			this.StartFrame = input.ReadValueF32(endianess);
			this.EndFrame = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.DofNear = input.ReadValueF32(endianess);
			this.DofFar = input.ReadValueF32(endianess);
			this.DofRange = input.ReadValueF32(endianess);
			this.DofAperture = input.ReadValueF32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
