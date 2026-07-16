using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.InsideReservedVolume)]
	public class InsideReservedVolumeCondition : P1Condition
	{
		public bool Inside { get; set; }
		public float Radius { get; set; }
		public bool Destination { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Inside, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueB32(this.Destination, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Inside = input.ReadValueB32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.Destination = input.ReadValueB32(endianess);
		}
	}
}
