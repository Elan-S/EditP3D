using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.CameraClippingPlanes)]
	public class CameraClippingPlanesTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float NearPlane { get; set; }
		public float FarPlane { get; set; }
		public bool RestoreAtEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.NearPlane, endianess);
			output.WriteValueF32(this.FarPlane, endianess);
			output.WriteValueB32(this.RestoreAtEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.NearPlane = input.ReadValueF32(endianess);
			this.FarPlane = input.ReadValueF32(endianess);
			this.RestoreAtEnd = input.ReadValueB32(endianess);
		}
	}
}
