using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(16830522410278814936UL)]
	public class SetPedestrianReadyForDowngradeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Ready { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Ready, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Ready = input.ReadValueB32(endianess);
		}
	}
}
