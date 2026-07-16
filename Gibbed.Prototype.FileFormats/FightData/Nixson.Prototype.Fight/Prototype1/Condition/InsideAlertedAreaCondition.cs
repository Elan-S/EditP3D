using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Alert)]
	[KnownCondition(11254368248701206822UL)]
	public class InsideAlertedAreaCondition : P1Condition
	{
		public bool Inside { get; set; }
		public bool PlayerOnly { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Inside, endianess);
			output.WriteValueB32(this.PlayerOnly, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Inside = input.ReadValueB32(endianess);
			this.PlayerOnly = input.ReadValueB32(endianess);
		}
	}
}
