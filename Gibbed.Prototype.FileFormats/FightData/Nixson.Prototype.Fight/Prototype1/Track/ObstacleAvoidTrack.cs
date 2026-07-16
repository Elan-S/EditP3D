using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.ObstacleAvoid)]
	public class ObstacleAvoidTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float HeightMin { get; set; }
		public float UpVelocityMax { get; set; }
		public float ForwardVelocityMin { get; set; }
		public float ForwardVelocityMax { get; set; }
		public float TurnVelocityMin { get; set; }
		public float TurnVelocityMax { get; set; }
		public float TurnAccelerationMin { get; set; }
		public float TurnAccelerationMax { get; set; }
		public float PoleAvoidVelocity { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.HeightMin, endianess);
			output.WriteValueF32(this.UpVelocityMax, endianess);
			output.WriteValueF32(this.ForwardVelocityMin, endianess);
			output.WriteValueF32(this.ForwardVelocityMax, endianess);
			output.WriteValueF32(this.TurnVelocityMin, endianess);
			output.WriteValueF32(this.TurnVelocityMax, endianess);
			output.WriteValueF32(this.TurnAccelerationMin, endianess);
			output.WriteValueF32(this.TurnAccelerationMax, endianess);
			output.WriteValueF32(this.PoleAvoidVelocity, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.HeightMin = input.ReadValueF32(endianess);
			this.UpVelocityMax = input.ReadValueF32(endianess);
			this.ForwardVelocityMin = input.ReadValueF32(endianess);
			this.ForwardVelocityMax = input.ReadValueF32(endianess);
			this.TurnVelocityMin = input.ReadValueF32(endianess);
			this.TurnVelocityMax = input.ReadValueF32(endianess);
			this.TurnAccelerationMin = input.ReadValueF32(endianess);
			this.TurnAccelerationMax = input.ReadValueF32(endianess);
			this.PoleAvoidVelocity = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
