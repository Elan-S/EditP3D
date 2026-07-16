using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17729545483452531168UL)]
	public class GameObjectOrientTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MaxTurnYaw { get; set; }
		public float MaxTurnPitch { get; set; }
		public bool Yaw { get; set; }
		public bool Pitch { get; set; }
		public bool ResetIfNotSetting { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MaxTurnYaw, endianess);
			output.WriteValueF32(this.MaxTurnPitch, endianess);
			output.WriteValueB32(this.Yaw, endianess);
			output.WriteValueB32(this.Pitch, endianess);
			output.WriteValueB32(this.ResetIfNotSetting, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MaxTurnYaw = input.ReadValueF32(endianess);
			this.MaxTurnPitch = input.ReadValueF32(endianess);
			this.Yaw = input.ReadValueB32(endianess);
			this.Pitch = input.ReadValueB32(endianess);
			this.ResetIfNotSetting = input.ReadValueB32(endianess);
		}
	}
}
