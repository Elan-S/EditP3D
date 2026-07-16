using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(12023123711471740387UL)]
	public class SupportingLimbTestSwitchSurfacesCondition : P1Condition
	{
		public bool SwitchSurfaces { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.SwitchSurfaces, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SwitchSurfaces = input.ReadValueB32(endianess);
		}
	}
}
