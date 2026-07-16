using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(TrackHash.StateMachineStart)]
	public class StateMachineStartTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Name { get; set; }
		public string Args { get; set; }
		public string Tags { get; set; }
		public string Channel { get; set; }
		public BranchReference StateMachineBranchRef { get; set; } = new BranchReference();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Name, endianess);
			output.WriteStringU32(this.Args, endianess);
			output.WriteStringU32(this.Tags, endianess);
			output.WriteStringU32(this.Channel, endianess);
			this.StateMachineBranchRef.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Name = input.ReadValueU64(endianess);
			this.Args = input.ReadStringAlignedU32(endianess);
			this.Tags = input.ReadStringAlignedU32(endianess);
			this.Channel = input.ReadStringAlignedU32(endianess);
			this.StateMachineBranchRef = new BranchReference(input, endianess);
		}
	}
}
