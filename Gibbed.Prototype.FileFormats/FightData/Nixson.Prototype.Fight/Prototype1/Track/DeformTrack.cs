using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(18039437269397831667UL)]
	public class DeformTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Joint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Direction { get; set; } = new Vector();
		public float Radius { get; set; }
		public float BlendTime { get; set; }
		public float DeformDirStr { get; set; }
		public float Strength { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
			output.WriteValueF32(this.DeformDirStr, endianess);
			output.WriteValueF32(this.Strength, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Offset.Deserialize(input, endianess);
			this.Direction.Deserialize(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.BlendTime = input.ReadValueF32(endianess);
			this.DeformDirStr = input.ReadValueF32(endianess);
			this.Strength = input.ReadValueF32(endianess);
		}
	}
}
