using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.VehiclePowerOnOff)]
	public class VehiclePowerOnOffTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool PowerOn { get; set; }
		public bool UseSpinup { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.PowerOn, endianess);
			output.WriteValueB32(this.UseSpinup, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.PowerOn = input.ReadValueB32(endianess);
			this.UseSpinup = input.ReadValueB32(endianess);
		}
	}
}
