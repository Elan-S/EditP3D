using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ForceMotion)]
	public class ForceMotionTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public Vector Motion { get; set; } = new Vector();
		public bool Local { get; set; }
		public float AccelerationFactor { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.Motion.Serialize(output, endianess);
			output.WriteValueB32(this.Local, endianess);
			output.WriteValueF32(this.AccelerationFactor, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Motion.Deserialize(input, endianess);
			this.Local = input.ReadValueB32(endianess);
			this.AccelerationFactor = input.ReadValueF32(endianess);
		}
	}
}
