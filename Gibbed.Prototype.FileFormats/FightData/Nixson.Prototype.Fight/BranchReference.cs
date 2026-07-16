using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight
{
	public class BranchReference
	{
		public string Name { get; set; } = "";
		public int Index { get; set; } = -1;
		public BranchReference()
		{
		}
		public BranchReference(Stream input, Endian endianess)
		{
			this.Deserialize(input, endianess);
		}
		public void Deserialize(Stream input, Endian endianess)
		{
			this.Name = input.ReadStringAlignedU32(endianess);
			this.Index = input.ReadValueS32(endianess);
		}
		public void Serialize(Stream output, Endian endianess)
		{
			output.WriteStringAlignedU32(this.Name, endianess);
			output.WriteValueS32(this.Index, endianess);
		}
	}
}
