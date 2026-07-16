using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.DebugSphere)]
	public class DebugSphereTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Radius { get; set; }
		public ulong Joint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Color Colour { get; set; } = new Color();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Colour.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Offset.Deserialize(input, endianess);
			this.Colour.Deserialize(input, endianess);
		}
	}
}
