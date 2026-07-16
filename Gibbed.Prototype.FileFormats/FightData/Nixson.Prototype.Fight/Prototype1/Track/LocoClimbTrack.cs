using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.LocoClimb)]
	public class LocoClimbTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Acceleration { get; set; }
		public float VelocityNorth { get; set; }
		public float VelocityStrafe { get; set; }
		public float VelocitySouth { get; set; }
		public float Phase { get; set; }
		public ulong Locomotion { get; set; }
		public ulong AnimIdle { get; set; }
		public ulong AnimNorth { get; set; }
		public ulong AnimEast { get; set; }
		public ulong AnimSouth { get; set; }
		public ulong AnimWest { get; set; }
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
			output.WriteValueF32(this.VelocityNorth, endianess);
			output.WriteValueF32(this.VelocityStrafe, endianess);
			output.WriteValueF32(this.VelocitySouth, endianess);
			output.WriteValueF32(this.Phase, endianess);
			output.WriteValueU64(this.Locomotion, endianess);
			output.WriteValueU64(this.AnimIdle, endianess);
			output.WriteValueU64(this.AnimNorth, endianess);
			output.WriteValueU64(this.AnimEast, endianess);
			output.WriteValueU64(this.AnimSouth, endianess);
			output.WriteValueU64(this.AnimWest, endianess);
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
			this.VelocityNorth = input.ReadValueF32(endianess);
			this.VelocityStrafe = input.ReadValueF32(endianess);
			this.VelocitySouth = input.ReadValueF32(endianess);
			this.Phase = input.ReadValueF32(endianess);
			this.Locomotion = input.ReadValueU64(endianess);
			this.AnimIdle = input.ReadValueU64(endianess);
			this.AnimNorth = input.ReadValueU64(endianess);
			this.AnimEast = input.ReadValueU64(endianess);
			this.AnimSouth = input.ReadValueU64(endianess);
			this.AnimWest = input.ReadValueU64(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
