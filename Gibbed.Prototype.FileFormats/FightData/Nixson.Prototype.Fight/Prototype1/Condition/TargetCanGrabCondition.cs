using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(16482230634334472989UL)]
	public class TargetCanGrabCondition : P1Condition
	{
		public bool SetGrabTarget { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.SetGrabTarget, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SetGrabTarget = input.ReadValueB32(endianess);
		}
	}
}
