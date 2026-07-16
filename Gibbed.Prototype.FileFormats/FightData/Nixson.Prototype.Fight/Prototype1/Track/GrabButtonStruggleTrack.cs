using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.GrabButtonStruggle)]
	public class GrabButtonStruggleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float TimeInput { get; set; }
		public ulong GrabSlot { get; set; }
		public float InitialCharge { get; set; }
		public float AutoChargeRateMin { get; set; }
		public float AutoChargeRateMax { get; set; }
		public float ButtonChargeRate { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.TimeInput, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueF32(this.InitialCharge, endianess);
			output.WriteValueF32(this.AutoChargeRateMin, endianess);
			output.WriteValueF32(this.AutoChargeRateMax, endianess);
			output.WriteValueF32(this.ButtonChargeRate, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.TimeInput = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.InitialCharge = input.ReadValueF32(endianess);
			this.AutoChargeRateMin = input.ReadValueF32(endianess);
			this.AutoChargeRateMax = input.ReadValueF32(endianess);
			this.ButtonChargeRate = input.ReadValueF32(endianess);
		}
	}
}
