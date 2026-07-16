using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(9994889469496860061UL)]
	public class ShootCurrentWeaponConfigurationCondition : P1Condition
	{
		public ulong WeaponConfiguration { get; set; }
		public bool Match { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.WeaponConfiguration, endianess);
			output.WriteValueB32(this.Match, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.WeaponConfiguration = input.ReadValueU64(endianess);
			this.Match = input.ReadValueB32(endianess);
		}
	}
}
