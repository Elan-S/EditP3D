using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(14925366827448248309UL)]
	public class PackageReadyCondition : P1Condition
	{
		public string PackageName { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteStringAlignedU32(this.PackageName, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.PackageName = input.ReadStringAlignedU32(endianess);
		}
	}
}
