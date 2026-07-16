using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SetVehicleSolverEnabled)]
	public class SetVehicleSolverEnabledTrack : P1Track
	{
		public bool Enabled { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Enabled, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Enabled = input.ReadValueB32(endianess);
		}
	}
}
