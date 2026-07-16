using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.LocoSteerLean)]
	public class LocoSteerLeanTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Acceleration { get; set; }
		public float TurningVelocity { get; set; }
		public float VelocityWalk { get; set; }
		public float VelocityRun { get; set; }
		public float LeanAngleThreshold { get; set; }
		public float LeanRateIn { get; set; }
		public float LeanRateOut { get; set; }
		public bool UseWorldCoordinates { get; set; }
		public float Phase { get; set; }
		public ulong Locomotion { get; set; }
		public ulong AnimIdle { get; set; }
		public ulong AnimWalk { get; set; }
		public ulong AnimWalkLeanEast { get; set; }
		public ulong AnimWalkLeanWest { get; set; }
		public ulong AnimRun { get; set; }
		public ulong AnimRunLeanEast { get; set; }
		public ulong AnimRunLeanWest { get; set; }
		public float SyncFrameIdle { get; set; }
		public float SyncFrameWalk { get; set; }
		public float SyncFrameWalkLeanEast { get; set; }
		public float SyncFrameWalkLeanWest { get; set; }
		public float SyncFrameRun { get; set; }
		public float SyncFrameRunLeanEast { get; set; }
		public float SyncFrameRunLeanWest { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.TurningVelocity, endianess);
			output.WriteValueF32(this.VelocityWalk, endianess);
			output.WriteValueF32(this.VelocityRun, endianess);
			output.WriteValueF32(this.LeanAngleThreshold, endianess);
			output.WriteValueF32(this.LeanRateIn, endianess);
			output.WriteValueF32(this.LeanRateOut, endianess);
			output.WriteValueB32(this.UseWorldCoordinates, endianess);
			output.WriteValueF32(this.Phase, endianess);
			output.WriteValueU64(this.Locomotion, endianess);
			output.WriteValueU64(this.AnimIdle, endianess);
			output.WriteValueU64(this.AnimWalk, endianess);
			output.WriteValueU64(this.AnimWalkLeanEast, endianess);
			output.WriteValueU64(this.AnimWalkLeanWest, endianess);
			output.WriteValueU64(this.AnimRun, endianess);
			output.WriteValueU64(this.AnimRunLeanEast, endianess);
			output.WriteValueU64(this.AnimRunLeanWest, endianess);
			output.WriteValueF32(this.SyncFrameIdle, endianess);
			output.WriteValueF32(this.SyncFrameWalk, endianess);
			output.WriteValueF32(this.SyncFrameWalkLeanEast, endianess);
			output.WriteValueF32(this.SyncFrameWalkLeanWest, endianess);
			output.WriteValueF32(this.SyncFrameRun, endianess);
			output.WriteValueF32(this.SyncFrameRunLeanEast, endianess);
			output.WriteValueF32(this.SyncFrameRunLeanWest, endianess);
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
			this.Acceleration = input.ReadValueF32(endianess);
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.VelocityWalk = input.ReadValueF32(endianess);
			this.VelocityRun = input.ReadValueF32(endianess);
			this.LeanAngleThreshold = input.ReadValueF32(endianess);
			this.LeanRateIn = input.ReadValueF32(endianess);
			this.LeanRateOut = input.ReadValueF32(endianess);
			this.UseWorldCoordinates = input.ReadValueB32(endianess);
			this.Phase = input.ReadValueF32(endianess);
			this.Locomotion = input.ReadValueU64(endianess);
			this.AnimIdle = input.ReadValueU64(endianess);
			this.AnimWalk = input.ReadValueU64(endianess);
			this.AnimWalkLeanEast = input.ReadValueU64(endianess);
			this.AnimWalkLeanWest = input.ReadValueU64(endianess);
			this.AnimRun = input.ReadValueU64(endianess);
			this.AnimRunLeanEast = input.ReadValueU64(endianess);
			this.AnimRunLeanWest = input.ReadValueU64(endianess);
			this.SyncFrameIdle = input.ReadValueF32(endianess);
			this.SyncFrameWalk = input.ReadValueF32(endianess);
			this.SyncFrameWalkLeanEast = input.ReadValueF32(endianess);
			this.SyncFrameWalkLeanWest = input.ReadValueF32(endianess);
			this.SyncFrameRun = input.ReadValueF32(endianess);
			this.SyncFrameRunLeanEast = input.ReadValueF32(endianess);
			this.SyncFrameRunLeanWest = input.ReadValueF32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
