using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.CircleStrafe)]
	public class CircleStrafeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float DistanceMin { get; set; }
		public float DistanceMax { get; set; }
		public float HeightMin { get; set; }
		public float HeightMax { get; set; }
		public float SafeHeight { get; set; }
		public float MinSpeed { get; set; }
		public float MaxSpeed { get; set; }
		public float ShootingDistance { get; set; }
		public bool UseMissiles { get; set; }
		public float MinFireTime { get; set; }
		public float MaxFireTime { get; set; }
		public float MinWaitTime { get; set; }
		public float MaxWaitTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.DistanceMin, endianess);
			output.WriteValueF32(this.DistanceMax, endianess);
			output.WriteValueF32(this.HeightMin, endianess);
			output.WriteValueF32(this.HeightMax, endianess);
			output.WriteValueF32(this.SafeHeight, endianess);
			output.WriteValueF32(this.MinSpeed, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			output.WriteValueF32(this.ShootingDistance, endianess);
			output.WriteValueB32(this.UseMissiles, endianess);
			output.WriteValueF32(this.MinFireTime, endianess);
			output.WriteValueF32(this.MaxFireTime, endianess);
			output.WriteValueF32(this.MinWaitTime, endianess);
			output.WriteValueF32(this.MaxWaitTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.DistanceMin = input.ReadValueF32(endianess);
			this.DistanceMax = input.ReadValueF32(endianess);
			this.HeightMin = input.ReadValueF32(endianess);
			this.HeightMax = input.ReadValueF32(endianess);
			this.SafeHeight = input.ReadValueF32(endianess);
			this.MinSpeed = input.ReadValueF32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.ShootingDistance = input.ReadValueF32(endianess);
			this.UseMissiles = input.ReadValueB32(endianess);
			this.MinFireTime = input.ReadValueF32(endianess);
			this.MaxFireTime = input.ReadValueF32(endianess);
			this.MinWaitTime = input.ReadValueF32(endianess);
			this.MaxWaitTime = input.ReadValueF32(endianess);
		}
	}
}
