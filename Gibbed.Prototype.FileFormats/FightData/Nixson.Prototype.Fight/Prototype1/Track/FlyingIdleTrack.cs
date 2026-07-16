using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14649913327971192015UL)]
	public class FlyingIdleTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinHeight { get; set; }
		public float FreeRadius { get; set; }
		public float Tolerance { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MinHeight, endianess);
			output.WriteValueF32(this.FreeRadius, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MinHeight = input.ReadValueF32(endianess);
			this.FreeRadius = input.ReadValueF32(endianess);
			this.Tolerance = input.ReadValueF32(endianess);
		}
	}
}
