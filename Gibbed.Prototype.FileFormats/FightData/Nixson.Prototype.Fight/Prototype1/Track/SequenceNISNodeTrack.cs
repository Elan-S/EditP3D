using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10198177156032706251UL)]
	public class SequenceNISNodeTrack : P1Track
	{
		public BranchReference Branch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Branch.Deserialize(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
	}
}
