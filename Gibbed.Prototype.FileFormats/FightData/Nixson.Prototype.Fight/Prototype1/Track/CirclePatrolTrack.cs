using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.CirclePatrol)]
	public class CirclePatrolTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float HeightMin { get; set; }
		public float HeightMax { get; set; }
		public float Radius { get; set; }
		public float FreeRadius { get; set; }
		public float AvoidRadius { get; set; }
		public float AvoidCost { get; set; }
		public int Tries { get; set; }
		public float MaxSpeed { get; set; }
		public float MinDistance { get; set; }
		public float MinAngle { get; set; }
		public float MaxPathHeightDiffCurrent { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.HeightMin, endianess);
			output.WriteValueF32(this.HeightMax, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.FreeRadius, endianess);
			output.WriteValueF32(this.AvoidRadius, endianess);
			output.WriteValueF32(this.AvoidCost, endianess);
			output.WriteValueS32(this.Tries, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			output.WriteValueF32(this.MinDistance, endianess);
			output.WriteValueF32(this.MinAngle, endianess);
			output.WriteValueF32(this.MaxPathHeightDiffCurrent, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.HeightMin = input.ReadValueF32(endianess);
			this.HeightMax = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.FreeRadius = input.ReadValueF32(endianess);
			this.AvoidRadius = input.ReadValueF32(endianess);
			this.AvoidCost = input.ReadValueF32(endianess);
			this.Tries = input.ReadValueS32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.MinDistance = input.ReadValueF32(endianess);
			this.MinAngle = input.ReadValueF32(endianess);
			this.MaxPathHeightDiffCurrent = input.ReadValueF32(endianess);
		}
	}
}
