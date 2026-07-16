using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ConfigureScaryMonster)]
	public class ConfigureScaryMonsterBehaviourTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Intensity { get; set; }
		public float VelocityOffset { get; set; }
		public int Frequency { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Intensity, endianess);
			output.WriteValueF32(this.VelocityOffset, endianess);
			output.WriteValueS32(this.Frequency, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Intensity = input.ReadValueF32(endianess);
			this.VelocityOffset = input.ReadValueF32(endianess);
			this.Frequency = input.ReadValueS32(endianess);
		}
	}
}
