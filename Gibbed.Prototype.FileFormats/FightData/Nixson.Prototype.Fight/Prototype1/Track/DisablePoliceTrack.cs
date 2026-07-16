using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.DisablePolice)]
	public class DisablePoliceTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Disable { get; set; }
		public float Delay { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Disable, endianess);
			output.WriteValueF32(this.Delay, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Disable = input.ReadValueB32(endianess);
			this.Delay = input.ReadValueF32(endianess);
		}
	}
}
