using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SetUserTooltip)]
	public class SetUserTooltipTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public string Tooltip { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteStringAlignedU32(this.Tooltip, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Tooltip = input.ReadStringAlignedU32(endianess);
		}
	}
}
