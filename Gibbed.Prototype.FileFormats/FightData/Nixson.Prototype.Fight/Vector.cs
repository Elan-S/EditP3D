using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight
{
	public class Vector
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public Vector()
		{
		}
		public Vector(Stream input, Endian endianess)
		{
			this.Deserialize(input, endianess);
		}
		public void Deserialize(Stream input, Endian endianess)
		{
			this.X = input.ReadValueF32(endianess);
			this.Y = input.ReadValueF32(endianess);
			this.Z = input.ReadValueF32(endianess);
		}
		public void Serialize(Stream output, Endian endianess)
		{
			output.WriteValueF32(this.X, endianess);
			output.WriteValueF32(this.Y, endianess);
			output.WriteValueF32(this.Z, endianess);
		}
	}
}
