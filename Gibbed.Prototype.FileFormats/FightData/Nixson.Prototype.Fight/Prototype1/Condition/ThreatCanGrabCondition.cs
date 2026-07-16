using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.ThreatCanGrab)]
	public class ThreatCanGrabCondition : P1Condition
	{
		public ulong Name { get; set; }
		public bool SetGrabTarget { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Name, endianess);
			output.WriteValueB32(this.SetGrabTarget, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.SetGrabTarget = input.ReadValueB32(endianess);
		}
	}
}
