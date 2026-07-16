using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(17529481764968080434UL)]
	public class TransformationDesiredSensesCondition : P1Condition
	{
		public bool ThermalVisionDesired { get; set; }
		public bool InfectedVisionDesired { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.ThermalVisionDesired, endianess);
			output.WriteValueB32(this.InfectedVisionDesired, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ThermalVisionDesired = input.ReadValueB32(endianess);
			this.InfectedVisionDesired = input.ReadValueB32(endianess);
		}
	}
}
