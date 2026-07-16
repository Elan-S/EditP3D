using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Platform)]
	public class PlatformCondition : P1Condition
	{
		public bool PS2 { get; set; }
		public bool XBox { get; set; }
		public bool GameCube { get; set; }
		public bool Win32 { get; set; }
		public bool Xenon { get; set; }
		public bool PS3 { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.PS2, endianess);
			output.WriteValueB32(this.XBox, endianess);
			output.WriteValueB32(this.GameCube, endianess);
			output.WriteValueB32(this.Win32, endianess);
			output.WriteValueB32(this.Xenon, endianess);
			output.WriteValueB32(this.PS3, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.PS2 = input.ReadValueB32(endianess);
			this.XBox = input.ReadValueB32(endianess);
			this.GameCube = input.ReadValueB32(endianess);
			this.Win32 = input.ReadValueB32(endianess);
			this.Xenon = input.ReadValueB32(endianess);
			this.PS3 = input.ReadValueB32(endianess);
		}
	}
}
