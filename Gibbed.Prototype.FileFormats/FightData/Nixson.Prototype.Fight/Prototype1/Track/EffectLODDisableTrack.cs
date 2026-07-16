using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11704847473920096712UL)]
	public class EffectLODDisableTrack : P1Track
	{
		public float LodStartDistance { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.LodStartDistance, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.LodStartDistance = input.ReadValueF32(endianess);
		}
	}
}
