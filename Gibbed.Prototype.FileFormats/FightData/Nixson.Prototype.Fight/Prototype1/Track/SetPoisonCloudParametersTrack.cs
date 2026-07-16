using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(12985568377929135875UL)]
	public class SetPoisonCloudParametersTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool GrowFacingDir { get; set; }
		public bool GrowDownDir { get; set; }
		public bool InheritFacingDir { get; set; }
		public float FallToGroundSpeed { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.GrowFacingDir, endianess);
			output.WriteValueB32(this.GrowDownDir, endianess);
			output.WriteValueB32(this.InheritFacingDir, endianess);
			output.WriteValueF32(this.FallToGroundSpeed, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.GrowFacingDir = input.ReadValueB32(endianess);
			this.GrowDownDir = input.ReadValueB32(endianess);
			this.InheritFacingDir = input.ReadValueB32(endianess);
			this.FallToGroundSpeed = input.ReadValueF32(endianess);
		}
	}
}
