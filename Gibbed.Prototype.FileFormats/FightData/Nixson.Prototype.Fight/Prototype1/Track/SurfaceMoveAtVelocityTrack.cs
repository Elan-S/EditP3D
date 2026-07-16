using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.SurfaceMoveAtVelocity)]
	public class SurfaceMoveAtVelocityTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public Vector Velocity { get; set; } = new Vector();
		public float Orientation { get; set; }
		public bool PreserveMomentum { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.Velocity.Serialize(output, endianess);
			output.WriteValueF32(this.Orientation, endianess);
			output.WriteValueB32(this.PreserveMomentum, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Velocity = new Vector(input, endianess);
			this.Orientation = input.ReadValueF32(endianess);
			this.PreserveMomentum = input.ReadValueB32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
