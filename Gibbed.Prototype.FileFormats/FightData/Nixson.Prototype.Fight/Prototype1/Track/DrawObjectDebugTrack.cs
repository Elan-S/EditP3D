using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13087879320114625660UL)]
	public class DrawObjectDebugTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public Color Colour { get; set; } = new Color();
		public Vector Offset { get; set; } = new Vector();
		public ulong AttachJoint { get; set; }
		public float Radius { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.Colour.Serialize(output, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueU64(this.AttachJoint, endianess);
			output.WriteValueF32(this.Radius, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Colour.Deserialize(input, endianess);
			this.Offset.Deserialize(input, endianess);
			this.AttachJoint = input.ReadValueU64(endianess);
			this.Radius = input.ReadValueF32(endianess);
		}
	}
}
