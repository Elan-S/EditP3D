using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14431228175762052182UL)]
	public class FixedDestinationFlytoTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Tolerance { get; set; }
		public bool Braking { get; set; }
		public bool IgnoreRestrictions { get; set; }
		public bool ClearDestination { get; set; }
		public float FreeArea { get; set; }
		public float MinHeight { get; set; }
		public float MaxSpeed { get; set; }
		public Vector SpeedFactor { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
			output.WriteValueB32(this.Braking, endianess);
			output.WriteValueB32(this.IgnoreRestrictions, endianess);
			output.WriteValueB32(this.ClearDestination, endianess);
			output.WriteValueF32(this.FreeArea, endianess);
			output.WriteValueF32(this.MinHeight, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			this.SpeedFactor.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Tolerance = input.ReadValueF32(endianess);
			this.Braking = input.ReadValueB32(endianess);
			this.IgnoreRestrictions = input.ReadValueB32(endianess);
			this.ClearDestination = input.ReadValueB32(endianess);
			this.FreeArea = input.ReadValueF32(endianess);
			this.MinHeight = input.ReadValueF32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.SpeedFactor.Deserialize(input, endianess);
		}
	}
}
