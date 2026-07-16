using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.DumbGotoLOD)]
	public class DumbGotoLODTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Tolerance { get; set; }
		public float BrakingDistance { get; set; }
		public bool ClearDestination { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
			output.WriteValueF32(this.BrakingDistance, endianess);
			output.WriteValueB32(this.ClearDestination, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Tolerance = input.ReadValueF32(endianess);
			this.BrakingDistance = input.ReadValueF32(endianess);
			this.ClearDestination = input.ReadValueB32(endianess);
		}
	}
}
