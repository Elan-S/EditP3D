using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11712535588566138471UL)]
	public class WasShotByPlayerCondition : P1Condition
	{
		public bool ShotByPlayer { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.ShotByPlayer, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ShotByPlayer = input.ReadValueB32(endianess);
		}
	}
}
