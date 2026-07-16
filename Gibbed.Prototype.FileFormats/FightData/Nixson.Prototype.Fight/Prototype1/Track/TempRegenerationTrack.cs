using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13628148291239217287UL)]
	public class TempRegenerationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float RegenRate { get; set; }
		public float RegenMaxHealthPercentage { get; set; }
		public float Expiry { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.RegenRate, endianess);
			output.WriteValueF32(this.RegenMaxHealthPercentage, endianess);
			output.WriteValueF32(this.Expiry, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.RegenRate = input.ReadValueF32(endianess);
			this.RegenMaxHealthPercentage = input.ReadValueF32(endianess);
			this.Expiry = input.ReadValueF32(endianess);
		}
	}
}
