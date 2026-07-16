using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.IntersectionCirclePatrol)]
	public class IntersectionCirclePatrolTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float HeightMin { get; set; }
		public float HeightMinFromTarget { get; set; }
		public float MinDistance { get; set; }
		public float Radius { get; set; }
		public float FreeRadiusPath { get; set; }
		public float FreeRadius { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.HeightMin, endianess);
			output.WriteValueF32(this.HeightMinFromTarget, endianess);
			output.WriteValueF32(this.MinDistance, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.FreeRadiusPath, endianess);
			output.WriteValueF32(this.FreeRadius, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.HeightMin = input.ReadValueF32(endianess);
			this.HeightMinFromTarget = input.ReadValueF32(endianess);
			this.MinDistance = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.FreeRadiusPath = input.ReadValueF32(endianess);
			this.FreeRadius = input.ReadValueF32(endianess);
		}
	}
}
