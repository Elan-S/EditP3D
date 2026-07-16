using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.PredefinedPatrol)]
	public class PredefinedPatrolTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float FreeRadius { get; set; }
		public float PrimaryLength { get; set; }
		public float SecondaryLength { get; set; }
		public float FreeRadiusPath { get; set; }
		public float DistanceToStartTolerance { get; set; }
		public float DistanceTolerance { get; set; }
		public float MaxPitch { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.FreeRadius, endianess);
			output.WriteValueF32(this.PrimaryLength, endianess);
			output.WriteValueF32(this.SecondaryLength, endianess);
			output.WriteValueF32(this.FreeRadiusPath, endianess);
			output.WriteValueF32(this.DistanceToStartTolerance, endianess);
			output.WriteValueF32(this.DistanceTolerance, endianess);
			output.WriteValueF32(this.MaxPitch, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.FreeRadius = input.ReadValueF32(endianess);
			this.PrimaryLength = input.ReadValueF32(endianess);
			this.SecondaryLength = input.ReadValueF32(endianess);
			this.FreeRadiusPath = input.ReadValueF32(endianess);
			this.DistanceToStartTolerance = input.ReadValueF32(endianess);
			this.DistanceTolerance = input.ReadValueF32(endianess);
			this.MaxPitch = input.ReadValueF32(endianess);
		}
	}
}
