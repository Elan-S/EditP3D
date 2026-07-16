using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17725638261116027775UL)]
	public class TargetTokenBoostTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public int ExtraTokens { get; set; }
		public float BoostTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueS32(this.ExtraTokens, endianess);
			output.WriteValueF32(this.BoostTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ExtraTokens = input.ReadValueS32(endianess);
			this.BoostTime = input.ReadValueF32(endianess);
		}
	}
}
