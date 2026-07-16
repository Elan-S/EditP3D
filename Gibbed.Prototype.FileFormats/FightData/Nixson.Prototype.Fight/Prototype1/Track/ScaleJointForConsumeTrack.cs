using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ScaleJointForConsume)]
	public class ScaleJointForConsumeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong JointToScale { get; set; }
		public float DesiredScale { get; set; }
		public float ScaleRate { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.JointToScale, endianess);
			output.WriteValueF32(this.DesiredScale, endianess);
			output.WriteValueF32(this.ScaleRate, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.JointToScale = input.ReadValueU64(endianess);
			this.DesiredScale = input.ReadValueF32(endianess);
			this.ScaleRate = input.ReadValueF32(endianess);
		}
	}
}
