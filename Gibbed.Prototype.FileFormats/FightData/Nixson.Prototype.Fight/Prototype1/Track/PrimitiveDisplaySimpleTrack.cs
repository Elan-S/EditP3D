using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.PrimitiveDisplaySimple)]
	public class PrimitiveDisplaySimpleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong PrimitiveName { get; set; }
		public bool Visible { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.PrimitiveName, endianess);
			output.WriteValueB32(this.Visible, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.PrimitiveName = input.ReadValueU64(endianess);
			this.Visible = input.ReadValueB32(endianess);
		}
	}
}
