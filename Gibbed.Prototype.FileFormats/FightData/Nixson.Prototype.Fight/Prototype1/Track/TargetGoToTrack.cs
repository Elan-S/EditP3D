using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(16401152428732451830UL)]
	public class TargetGoToTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public TargetGoToTrack.WhereType Where { get; set; }
		public float Tolerance { get; set; }
		public float ToleranceRePathfind { get; set; }
		public float MinTimeBetweenPathfind { get; set; }
		public bool UseRandomOvershoot { get; set; }
		public float RandomOvershootLowerBound { get; set; }
		public float RandomOvershootUpperBound { get; set; }
		public bool Brake { get; set; }
		public bool IgnoreRestrictions { get; set; }
		public bool StayOnGround { get; set; }
		public bool ProjectPositionToGround { get; set; }
		public bool TargetClimbing { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<TargetGoToTrack.WhereType>(output, endianess, this.Where);
			output.WriteValueF32(this.Tolerance, endianess);
			output.WriteValueF32(this.ToleranceRePathfind, endianess);
			output.WriteValueF32(this.MinTimeBetweenPathfind, endianess);
			output.WriteValueB32(this.UseRandomOvershoot, endianess);
			output.WriteValueF32(this.RandomOvershootLowerBound, endianess);
			output.WriteValueF32(this.RandomOvershootUpperBound, endianess);
			output.WriteValueB32(this.Brake, endianess);
			output.WriteValueB32(this.IgnoreRestrictions, endianess);
			output.WriteValueB32(this.StayOnGround, endianess);
			output.WriteValueB32(this.ProjectPositionToGround, endianess);
			output.WriteValueB32(this.TargetClimbing, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Where = BaseProperty.DeserializePropertyEnum<TargetGoToTrack.WhereType>(input, endianess);
			this.Tolerance = input.ReadValueF32(endianess);
			this.ToleranceRePathfind = input.ReadValueF32(endianess);
			this.MinTimeBetweenPathfind = input.ReadValueF32(endianess);
			this.UseRandomOvershoot = input.ReadValueB32(endianess);
			this.RandomOvershootLowerBound = input.ReadValueF32(endianess);
			this.RandomOvershootUpperBound = input.ReadValueF32(endianess);
			this.Brake = input.ReadValueB32(endianess);
			this.IgnoreRestrictions = input.ReadValueB32(endianess);
			this.StayOnGround = input.ReadValueB32(endianess);
			this.ProjectPositionToGround = input.ReadValueB32(endianess);
			this.TargetClimbing = input.ReadValueB32(endianess);
		}
		public enum WhereType : ulong
		{
			Target = 856854631462190855UL,
			GrabTarget = 1754404701201221985UL
		}
	}
}
