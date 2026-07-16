using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SetEffectTimer)]
	public class SetEffectTimerTrack : P1Track
	{
		public ulong Name { get; set; }
		public float Time { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Name, endianess);
			output.WriteValueF32(this.Time, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.Time = input.ReadValueF32(endianess);
		}
	}
}
