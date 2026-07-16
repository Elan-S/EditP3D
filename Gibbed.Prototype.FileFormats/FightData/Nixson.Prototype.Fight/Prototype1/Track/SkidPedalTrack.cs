using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.SkidPedal)]
	public class SkidPedalTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEndMin { get; set; }
		public float TimeEnd { get; set; }
		public float Acceleration { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float TurnVelocityMin { get; set; }
		public float TurnVelocityMax { get; set; }
		public float TurnAccelerationMin { get; set; }
		public float TurnAccelerationMax { get; set; }
		public ulong AnimNorth { get; set; }
		public ulong AnimWest { get; set; }
		public ulong AnimSouth { get; set; }
		public ulong AnimEast { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEndMin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.TurnVelocityMin, endianess);
			output.WriteValueF32(this.TurnVelocityMax, endianess);
			output.WriteValueF32(this.TurnAccelerationMin, endianess);
			output.WriteValueF32(this.TurnAccelerationMax, endianess);
			output.WriteValueU64(this.AnimNorth, endianess);
			output.WriteValueU64(this.AnimWest, endianess);
			output.WriteValueU64(this.AnimSouth, endianess);
			output.WriteValueU64(this.AnimEast, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEndMin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Acceleration = input.ReadValueF32(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.TurnVelocityMin = input.ReadValueF32(endianess);
			this.TurnVelocityMax = input.ReadValueF32(endianess);
			this.TurnAccelerationMin = input.ReadValueF32(endianess);
			this.TurnAccelerationMax = input.ReadValueF32(endianess);
			this.AnimNorth = input.ReadValueU64(endianess);
			this.AnimWest = input.ReadValueU64(endianess);
			this.AnimSouth = input.ReadValueU64(endianess);
			this.AnimEast = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
