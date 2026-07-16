using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(17881508128190079531UL)]
	public class GrabbedByPlayerCondition : P1Condition
	{
		public bool IsGrabbedByPlayer { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.IsGrabbedByPlayer, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.IsGrabbedByPlayer = input.ReadValueB32(endianess);
		}
	}
}
