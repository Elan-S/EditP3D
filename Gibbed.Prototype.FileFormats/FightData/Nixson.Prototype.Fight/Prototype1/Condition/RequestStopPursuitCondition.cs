using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.RequestStopPursuit)]
	public class RequestStopPursuitCondition : P1Condition
	{
		public float MinActorAge { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.MinActorAge, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.MinActorAge = input.ReadValueF32(endianess);
		}
	}
}
