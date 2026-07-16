using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.WhipFistPullParentToTarget)]
	public class WhipFistPullParentToTargetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Velocity { get; set; }
		public float Acceleration { get; set; }
		public float ParabolicConstant { get; set; }
		public float ParabolicOffset { get; set; }
		public Vector GoalOffsetFromTarget { get; set; } = new Vector();
		public float FaceTargetMaxTurnRate { get; set; }
		public float MaxStuckTime { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Velocity, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.ParabolicConstant, endianess);
			output.WriteValueF32(this.ParabolicOffset, endianess);
			this.GoalOffsetFromTarget.Serialize(output, endianess);
			output.WriteValueF32(this.FaceTargetMaxTurnRate, endianess);
			output.WriteValueF32(this.MaxStuckTime, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Velocity = input.ReadValueF32(endianess);
			this.Acceleration = input.ReadValueF32(endianess);
			this.ParabolicConstant = input.ReadValueF32(endianess);
			this.ParabolicOffset = input.ReadValueF32(endianess);
			this.GoalOffsetFromTarget = new Vector(input, endianess);
			this.FaceTargetMaxTurnRate = input.ReadValueF32(endianess);
			this.MaxStuckTime = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
