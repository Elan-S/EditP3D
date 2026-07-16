using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SimpleOpenStrafeRun)]
	public class SimpleOpenStrafeRunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float FreeRadiusPath { get; set; }
		public float FreeRadiusPathToStart { get; set; }
		public float MinHeightStartFromTarget { get; set; }
		public float MinHeightGoUpStart { get; set; }
		public float ReserveVolumeMargin { get; set; }
		public float ReserveVolumeSideRadius { get; set; }
		public float ToleranceToStart { get; set; }
		public bool OrientAtEnd { get; set; }
		public Vector SpeedFactor { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.FreeRadiusPath, endianess);
			output.WriteValueF32(this.FreeRadiusPathToStart, endianess);
			output.WriteValueF32(this.MinHeightStartFromTarget, endianess);
			output.WriteValueF32(this.MinHeightGoUpStart, endianess);
			output.WriteValueF32(this.ReserveVolumeMargin, endianess);
			output.WriteValueF32(this.ReserveVolumeSideRadius, endianess);
			output.WriteValueF32(this.ToleranceToStart, endianess);
			output.WriteValueB32(this.OrientAtEnd, endianess);
			this.SpeedFactor.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.FreeRadiusPath = input.ReadValueF32(endianess);
			this.FreeRadiusPathToStart = input.ReadValueF32(endianess);
			this.MinHeightStartFromTarget = input.ReadValueF32(endianess);
			this.MinHeightGoUpStart = input.ReadValueF32(endianess);
			this.ReserveVolumeMargin = input.ReadValueF32(endianess);
			this.ReserveVolumeSideRadius = input.ReadValueF32(endianess);
			this.ToleranceToStart = input.ReadValueF32(endianess);
			this.OrientAtEnd = input.ReadValueB32(endianess);
			this.SpeedFactor.Deserialize(input, endianess);
		}
	}
}
