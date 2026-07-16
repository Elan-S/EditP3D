using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10784137945036409523UL)]
	public class FixedDestinationGotoTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Tolerance { get; set; }
		public bool Braking { get; set; }
		public bool IgnoreRestrictions { get; set; }
		public bool ClearDestination { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
			output.WriteValueB32(this.Braking, endianess);
			output.WriteValueB32(this.IgnoreRestrictions, endianess);
			output.WriteValueB32(this.ClearDestination, endianess);
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
		}
	}
}
