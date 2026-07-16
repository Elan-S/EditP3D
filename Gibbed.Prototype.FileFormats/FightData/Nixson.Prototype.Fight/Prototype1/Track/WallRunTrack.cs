using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.WallRun)]
	public class WallRunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float Acceleration { get; set; }
		public float Gravity { get; set; }
		public float TurningRate { get; set; }
		public float SurfaceAngleThreshold { get; set; }
		public ulong AnimUp { get; set; }
		public ulong AnimLeft { get; set; }
		public ulong AnimRight { get; set; }
		public ulong AnimUpLeft { get; set; }
		public ulong AnimUpRight { get; set; }
		public ulong AnimDownLeft { get; set; }
		public ulong AnimDownRight { get; set; }
		public bool ForceAnimationVelocities { get; set; }
		public float SyncFrameUp { get; set; }
		public float SyncFrameLeft { get; set; }
		public float SyncFrameRight { get; set; }
		public float SyncFrameUpLeft { get; set; }
		public float SyncFrameUpRight { get; set; }
		public float SyncFrameDownLeft { get; set; }
		public float SyncFrameDownRight { get; set; }
		public float UnlockableVelocityMin { get; set; }
		public UnlockableEnum UnlockableFirst { get; set; }
		public UnlockableEnum UnlockableLast { get; set; }
		public float SprintVelocityDropRate { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.TurningRate, endianess);
			output.WriteValueF32(this.SurfaceAngleThreshold, endianess);
			output.WriteValueU64(this.AnimUp, endianess);
			output.WriteValueU64(this.AnimLeft, endianess);
			output.WriteValueU64(this.AnimRight, endianess);
			output.WriteValueU64(this.AnimUpLeft, endianess);
			output.WriteValueU64(this.AnimUpRight, endianess);
			output.WriteValueU64(this.AnimDownLeft, endianess);
			output.WriteValueU64(this.AnimDownRight, endianess);
			output.WriteValueB32(this.ForceAnimationVelocities, endianess);
			output.WriteValueF32(this.SyncFrameUp, endianess);
			output.WriteValueF32(this.SyncFrameLeft, endianess);
			output.WriteValueF32(this.SyncFrameRight, endianess);
			output.WriteValueF32(this.SyncFrameUpLeft, endianess);
			output.WriteValueF32(this.SyncFrameUpRight, endianess);
			output.WriteValueF32(this.SyncFrameDownLeft, endianess);
			output.WriteValueF32(this.SyncFrameDownRight, endianess);
			output.WriteValueF32(this.UnlockableVelocityMin, endianess);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableFirst);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableLast);
			output.WriteValueF32(this.SprintVelocityDropRate, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.Acceleration = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.TurningRate = input.ReadValueF32(endianess);
			this.SurfaceAngleThreshold = input.ReadValueF32(endianess);
			this.AnimUp = input.ReadValueU64(endianess);
			this.AnimLeft = input.ReadValueU64(endianess);
			this.AnimRight = input.ReadValueU64(endianess);
			this.AnimUpLeft = input.ReadValueU64(endianess);
			this.AnimUpRight = input.ReadValueU64(endianess);
			this.AnimDownLeft = input.ReadValueU64(endianess);
			this.AnimDownRight = input.ReadValueU64(endianess);
			this.ForceAnimationVelocities = input.ReadValueB32(endianess);
			this.SyncFrameUp = input.ReadValueF32(endianess);
			this.SyncFrameLeft = input.ReadValueF32(endianess);
			this.SyncFrameRight = input.ReadValueF32(endianess);
			this.SyncFrameUpLeft = input.ReadValueF32(endianess);
			this.SyncFrameUpRight = input.ReadValueF32(endianess);
			this.SyncFrameDownLeft = input.ReadValueF32(endianess);
			this.SyncFrameDownRight = input.ReadValueF32(endianess);
			this.UnlockableVelocityMin = input.ReadValueF32(endianess);
			this.UnlockableFirst = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
			this.UnlockableLast = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
			this.SprintVelocityDropRate = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
