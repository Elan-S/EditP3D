using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.InHotZone)]
	public class InHotZoneCondition : P1Condition
	{
		public bool In { get; set; }
		public bool RequireStayOnGround { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.In, endianess);
			output.WriteValueB32(this.RequireStayOnGround, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.In = input.ReadValueB32(endianess);
			this.RequireStayOnGround = input.ReadValueB32(endianess);
		}
	}
}
