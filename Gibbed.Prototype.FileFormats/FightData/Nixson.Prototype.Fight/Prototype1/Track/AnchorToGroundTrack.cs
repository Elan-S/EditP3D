using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(9974631981605760061UL)]
	public class AnchorToGroundTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool Anchor { get; set; }
		public bool GroundLevel { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.Anchor, endianess);
			output.WriteValueB32(this.GroundLevel, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Anchor = input.ReadValueB32(endianess);
			this.GroundLevel = input.ReadValueB32(endianess);
		}
	}
}
