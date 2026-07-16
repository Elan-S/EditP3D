using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.VehicleReticule)]
	public class VehicleReticuleTrack : P1Track
	{
		public bool ReticuleOn { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.ReticuleOn, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ReticuleOn = input.ReadValueB32(endianess);
		}
	}
}
