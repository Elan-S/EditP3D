using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Land)]
	public class LandTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Speed { get; set; }
		public float ToleraceAngleStartLanding { get; set; }
		public bool ClearDestination { get; set; }
		public bool TurnOffRotor { get; set; }
		public bool IgnoreOrientation { get; set; }
		public float ToleranceY { get; set; }
		public float ToleranceXZ { get; set; }
		public float DistanceToGround { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueF32(this.ToleraceAngleStartLanding, endianess);
			output.WriteValueB32(this.ClearDestination, endianess);
			output.WriteValueB32(this.TurnOffRotor, endianess);
			output.WriteValueB32(this.IgnoreOrientation, endianess);
			output.WriteValueF32(this.ToleranceY, endianess);
			output.WriteValueF32(this.ToleranceXZ, endianess);
			output.WriteValueF32(this.DistanceToGround, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.ToleraceAngleStartLanding = input.ReadValueF32(endianess);
			this.ClearDestination = input.ReadValueB32(endianess);
			this.TurnOffRotor = input.ReadValueB32(endianess);
			this.IgnoreOrientation = input.ReadValueB32(endianess);
			this.ToleranceY = input.ReadValueF32(endianess);
			this.ToleranceXZ = input.ReadValueF32(endianess);
			this.DistanceToGround = input.ReadValueF32(endianess);
		}
	}
}
