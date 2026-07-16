using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17732735927699033291UL)]
	public class TempSpeedTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Speed { get; set; }
		public bool Sprint { get; set; }
		public bool UseMaxSpeed { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueB32(this.Sprint, endianess);
			output.WriteValueB32(this.UseMaxSpeed, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Sprint = input.ReadValueB32(endianess);
			this.UseMaxSpeed = input.ReadValueB32(endianess);
		}
	}
}
