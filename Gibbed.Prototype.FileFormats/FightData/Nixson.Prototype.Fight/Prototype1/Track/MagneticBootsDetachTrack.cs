using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.MagneticBootsDetach)]
	public class MagneticBootsDetachTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Time { get; set; }
		public float Separation { get; set; }
		public int ChoreoPriority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public bool DebugDraw { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Time, endianess);
			output.WriteValueF32(this.Separation, endianess);
			output.WriteValueS32(this.ChoreoPriority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			output.WriteValueB32(this.DebugDraw, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Time = input.ReadValueF32(endianess);
			this.Separation = input.ReadValueF32(endianess);
			this.ChoreoPriority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.DebugDraw = input.ReadValueB32(endianess);
		}
	}
}
