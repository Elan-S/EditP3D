using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.LocoSteer)]
	public class LocoSteerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Acceleration { get; set; }
		public float TurningVelocity { get; set; }
		public float VelocityWalk { get; set; }
		public float VelocityRun { get; set; }
		public float SmoothSteeringAngle { get; set; }
		public bool UseWorldCoordinates { get; set; }
		public float Phase { get; set; }
		public ulong Locomotion { get; set; }
		public ulong AnimIdle { get; set; }
		public ulong AnimWalk { get; set; }
		public ulong AnimRun { get; set; }
		public float SyncFrameIdle { get; set; }
		public float SyncFrameWalk { get; set; }
		public float SyncFrameRun { get; set; }
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
			output.WriteValueF32(this.SmoothSteeringAngle, endianess);
			output.WriteValueB32(this.UseWorldCoordinates, endianess);
			output.WriteValueF32(this.Phase, endianess);
			output.WriteValueU64(this.Locomotion, endianess);
			output.WriteValueU64(this.AnimIdle, endianess);
			output.WriteValueU64(this.AnimWalk, endianess);
			output.WriteValueU64(this.AnimRun, endianess);
			output.WriteValueF32(this.SyncFrameIdle, endianess);
			output.WriteValueF32(this.SyncFrameWalk, endianess);
			output.WriteValueF32(this.SyncFrameRun, endianess);
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
			this.SmoothSteeringAngle = input.ReadValueF32(endianess);
			this.UseWorldCoordinates = input.ReadValueB32(endianess);
			this.Phase = input.ReadValueF32(endianess);
			this.Locomotion = input.ReadValueU64(endianess);
			this.AnimIdle = input.ReadValueU64(endianess);
			this.AnimWalk = input.ReadValueU64(endianess);
			this.AnimRun = input.ReadValueU64(endianess);
			this.SyncFrameIdle = input.ReadValueF32(endianess);
			this.SyncFrameWalk = input.ReadValueF32(endianess);
			this.SyncFrameRun = input.ReadValueF32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
