using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(16126344776868143550UL)]
	public class TargetIsHumanCondition : P1Condition
	{
		public bool Human { get; set; }
		public bool CheckPointPeds { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Human, endianess);
			output.WriteValueB32(this.CheckPointPeds, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Human = input.ReadValueB32(endianess);
			this.CheckPointPeds = input.ReadValueB32(endianess);
		}
	}
}
