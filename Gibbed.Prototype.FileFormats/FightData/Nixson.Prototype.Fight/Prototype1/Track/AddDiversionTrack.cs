using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17127490303549609740UL)]
	public class AddDiversionTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Intensity { get; set; }
		public float MinDistance { get; set; }
		public float MaxDistance { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Intensity, endianess);
			output.WriteValueF32(this.MinDistance, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Intensity = input.ReadValueF32(endianess);
			this.MinDistance = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
		}
	}
}
