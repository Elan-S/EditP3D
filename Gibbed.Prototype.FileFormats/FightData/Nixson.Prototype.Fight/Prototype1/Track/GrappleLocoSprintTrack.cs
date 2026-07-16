using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10304918491597878848UL)]
	public class GrappleLocoSprintTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Locomotion { get; set; }
		public ulong AnimRun { get; set; }
		public ulong AnimLeanEast { get; set; }
		public ulong AnimLeanWest { get; set; }
		public float SyncFrameRun { get; set; }
		public float SyncFrameLeanEast { get; set; }
		public float SyncFrameLeanWest { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Locomotion, endianess);
			output.WriteValueU64(this.AnimRun, endianess);
			output.WriteValueU64(this.AnimLeanEast, endianess);
			output.WriteValueU64(this.AnimLeanWest, endianess);
			output.WriteValueF32(this.SyncFrameRun, endianess);
			output.WriteValueF32(this.SyncFrameLeanEast, endianess);
			output.WriteValueF32(this.SyncFrameLeanWest, endianess);
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
			this.Locomotion = input.ReadValueU64(endianess);
			this.AnimRun = input.ReadValueU64(endianess);
			this.AnimLeanEast = input.ReadValueU64(endianess);
			this.AnimLeanWest = input.ReadValueU64(endianess);
			this.SyncFrameRun = input.ReadValueF32(endianess);
			this.SyncFrameLeanEast = input.ReadValueF32(endianess);
			this.SyncFrameLeanWest = input.ReadValueF32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
