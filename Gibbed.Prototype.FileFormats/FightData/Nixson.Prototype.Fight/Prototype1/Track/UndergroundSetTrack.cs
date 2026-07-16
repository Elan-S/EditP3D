using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.UndergroundSet)]
	public class UndergroundSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Underground { get; set; }
		public bool Instantaneous { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Underground, endianess);
			output.WriteValueB32(this.Instantaneous, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Underground = input.ReadValueB32(endianess);
			this.Instantaneous = input.ReadValueB32(endianess);
		}
	}
}
