using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.JawFlap)]
	public class JawFlapTrack : P1Track
	{
		public float AngleMin { get; set; }
		public float AngleMax { get; set; }
		public float ThresholdMin { get; set; }
		public float ThresholdMax { get; set; }
		public ulong Joint { get; set; }
		public Vector Axis { get; set; } = new Vector();
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.AngleMin, endianess);
			output.WriteValueF32(this.AngleMax, endianess);
			output.WriteValueF32(this.ThresholdMin, endianess);
			output.WriteValueF32(this.ThresholdMax, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Axis.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.AngleMin = input.ReadValueF32(endianess);
			this.AngleMax = input.ReadValueF32(endianess);
			this.ThresholdMin = input.ReadValueF32(endianess);
			this.ThresholdMax = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Axis.Deserialize(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
