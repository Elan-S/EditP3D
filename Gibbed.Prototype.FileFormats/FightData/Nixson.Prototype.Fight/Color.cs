using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight
{
	public class Color
	{
		public float R { get; set; }
		public float G { get; set; }
		public float B { get; set; }
		public float A { get; set; }
		public Color()
		{
		}
		public Color(Stream input, Endian endianess)
		{
			this.Deserialize(input, endianess);
		}
		public void Deserialize(Stream input, Endian endianess)
		{
			this.R = input.ReadValueF32(endianess);
			this.G = input.ReadValueF32(endianess);
			this.B = input.ReadValueF32(endianess);
			this.A = input.ReadValueF32(endianess);
		}
		public void Serialize(Stream output, Endian endianess)
		{
			output.WriteValueF32(this.R, endianess);
			output.WriteValueF32(this.G, endianess);
			output.WriteValueF32(this.B, endianess);
			output.WriteValueF32(this.A, endianess);
		}
	}
}
