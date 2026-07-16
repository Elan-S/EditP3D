using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Branch
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownBranch(BranchHash.PlaybackSet)]
	public class PlaybackSet : BaseBranch
	{
		public ulong LogicTreeName { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.LogicTreeName, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.LogicTreeName = input.ReadValueU64(endianess);
		}
	}
}
