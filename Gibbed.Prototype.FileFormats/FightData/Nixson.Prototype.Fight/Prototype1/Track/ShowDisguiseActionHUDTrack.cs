using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(9331509862260394687UL)]
	public class ShowDisguiseActionHUDTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool ShowAndHideOnly { get; set; }
		public bool StealthConsumeInRange { get; set; }
		public bool StealthConsumeUsable { get; set; }
		public bool PatsyUsable { get; set; }
		public bool AirStrikeUsable { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.ShowAndHideOnly, endianess);
			output.WriteValueB32(this.StealthConsumeInRange, endianess);
			output.WriteValueB32(this.StealthConsumeUsable, endianess);
			output.WriteValueB32(this.PatsyUsable, endianess);
			output.WriteValueB32(this.AirStrikeUsable, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ShowAndHideOnly = input.ReadValueB32(endianess);
			this.StealthConsumeInRange = input.ReadValueB32(endianess);
			this.StealthConsumeUsable = input.ReadValueB32(endianess);
			this.PatsyUsable = input.ReadValueB32(endianess);
			this.AirStrikeUsable = input.ReadValueB32(endianess);
		}
	}
}
