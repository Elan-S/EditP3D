using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(9577247949001421875UL)]
	public class SwapDrawableTrack : P1Track
	{
		public ulong ObjectName { get; set; }
		public ulong DrawableName { get; set; }
		public bool Persistent { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.ObjectName, endianess);
			output.WriteValueU64(this.DrawableName, endianess);
			output.WriteValueB32(this.Persistent, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ObjectName = input.ReadValueU64(endianess);
			this.DrawableName = input.ReadValueU64(endianess);
			this.Persistent = input.ReadValueB32(endianess);
		}
	}
}
