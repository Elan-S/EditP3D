using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.LocoStrafe)]
	public class LocoStrafeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Acceleration { get; set; }
		public float TurningVelocity { get; set; }
		public bool FixedFacing { get; set; }
		public float VelocityWalkNorth { get; set; }
		public float VelocityWalkStrafe { get; set; }
		public float VelocityWalkSouth { get; set; }
		public float VelocityRunNorth { get; set; }
		public float VelocityRunStrafe { get; set; }
		public float VelocityRunSouth { get; set; }
		public float Phase { get; set; }
		public ulong Locomotion { get; set; }
		public ulong AnimIdle { get; set; }
		public ulong AnimWalkNorth { get; set; }
		public ulong AnimWalkEast { get; set; }
		public ulong AnimWalkSouth { get; set; }
		public ulong AnimWalkWest { get; set; }
		public ulong AnimRunNorth { get; set; }
		public ulong AnimRunEast { get; set; }
		public ulong AnimRunSouth { get; set; }
		public ulong AnimRunWest { get; set; }
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
			output.WriteValueB32(this.FixedFacing, endianess);
			output.WriteValueF32(this.VelocityWalkNorth, endianess);
			output.WriteValueF32(this.VelocityWalkStrafe, endianess);
			output.WriteValueF32(this.VelocityWalkSouth, endianess);
			output.WriteValueF32(this.VelocityRunNorth, endianess);
			output.WriteValueF32(this.VelocityRunStrafe, endianess);
			output.WriteValueF32(this.VelocityRunSouth, endianess);
			output.WriteValueF32(this.Phase, endianess);
			output.WriteValueU64(this.Locomotion, endianess);
			output.WriteValueU64(this.AnimIdle, endianess);
			output.WriteValueU64(this.AnimWalkNorth, endianess);
			output.WriteValueU64(this.AnimWalkEast, endianess);
			output.WriteValueU64(this.AnimWalkSouth, endianess);
			output.WriteValueU64(this.AnimWalkWest, endianess);
			output.WriteValueU64(this.AnimRunNorth, endianess);
			output.WriteValueU64(this.AnimRunEast, endianess);
			output.WriteValueU64(this.AnimRunSouth, endianess);
			output.WriteValueU64(this.AnimRunWest, endianess);
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
			this.FixedFacing = input.ReadValueB32(endianess);
			this.VelocityWalkNorth = input.ReadValueF32(endianess);
			this.VelocityWalkStrafe = input.ReadValueF32(endianess);
			this.VelocityWalkSouth = input.ReadValueF32(endianess);
			this.VelocityRunNorth = input.ReadValueF32(endianess);
			this.VelocityRunStrafe = input.ReadValueF32(endianess);
			this.VelocityRunSouth = input.ReadValueF32(endianess);
			this.Phase = input.ReadValueF32(endianess);
			this.Locomotion = input.ReadValueU64(endianess);
			this.AnimIdle = input.ReadValueU64(endianess);
			this.AnimWalkNorth = input.ReadValueU64(endianess);
			this.AnimWalkEast = input.ReadValueU64(endianess);
			this.AnimWalkSouth = input.ReadValueU64(endianess);
			this.AnimWalkWest = input.ReadValueU64(endianess);
			this.AnimRunNorth = input.ReadValueU64(endianess);
			this.AnimRunEast = input.ReadValueU64(endianess);
			this.AnimRunSouth = input.ReadValueU64(endianess);
			this.AnimRunWest = input.ReadValueU64(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
