using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(TrackHash.PackageLoad)]
	public class PackageLoadTrack : P1Track
	{
		public string PackageName { get; set; }
		public bool Async { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteStringAlignedU32(this.PackageName, endianess);
			output.WriteValueB32(this.Async, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.PackageName = input.ReadStringAlignedU32(endianess);
			this.Async = input.ReadValueB32(endianess);
		}
	}
}
