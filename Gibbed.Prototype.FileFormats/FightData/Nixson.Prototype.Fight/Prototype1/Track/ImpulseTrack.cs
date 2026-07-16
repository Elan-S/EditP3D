using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(15433622996739777765UL)]
	public class ImpulseTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public Vector Impulse { get; set; } = new Vector();
		public ulong Joint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Orientation { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			this.Impulse.Serialize(output, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Orientation.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Impulse = new Vector(input, endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.Orientation = new Vector(input, endianess);
		}
	}
}
