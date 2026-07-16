using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14476441972085345108UL)]
	public class SubTitleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public string SubTitle { get; set; }
		public string SubTitleType { get; set; }
		public string Speaker { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteStringAlignedU32(this.SubTitle, endianess);
			output.WriteStringAlignedU32(this.SubTitleType, endianess);
			output.WriteStringAlignedU32(this.Speaker, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.SubTitle = input.ReadStringAlignedU32(endianess);
			this.SubTitleType = input.ReadStringAlignedU32(endianess);
			this.Speaker = input.ReadStringAlignedU32(endianess);
		}
	}
}
