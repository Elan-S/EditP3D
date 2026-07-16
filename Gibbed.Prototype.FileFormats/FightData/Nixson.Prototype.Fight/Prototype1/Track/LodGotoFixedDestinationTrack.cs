using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(15168592712998017478UL)]
	public class LodGotoFixedDestinationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Speed { get; set; }
		public float VerticalSpeed { get; set; }
		public float Tolerance { get; set; }
		public float FreeArea { get; set; }
		public float MinHeight { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueF32(this.VerticalSpeed, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
			output.WriteValueF32(this.FreeArea, endianess);
			output.WriteValueF32(this.MinHeight, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.VerticalSpeed = input.ReadValueF32(endianess);
			this.Tolerance = input.ReadValueF32(endianess);
			this.FreeArea = input.ReadValueF32(endianess);
			this.MinHeight = input.ReadValueF32(endianess);
		}
	}
}
