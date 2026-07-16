using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12936713618574848070UL)]
	public class UninterruptibleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool IgnoreHits { get; set; }
		public bool IgnoreGrabs { get; set; }
		public bool AllowDevastatorGrabs { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.IgnoreHits, endianess);
			output.WriteValueB32(this.IgnoreGrabs, endianess);
			output.WriteValueB32(this.AllowDevastatorGrabs, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.IgnoreHits = input.ReadValueB32(endianess);
			this.IgnoreGrabs = input.ReadValueB32(endianess);
			this.AllowDevastatorGrabs = input.ReadValueB32(endianess);
		}
	}
}
