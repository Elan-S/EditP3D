using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(17374141745818903163UL)]
	public class VariableNameSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Variable { get; set; }
		public ulong Value { get; set; }
		public bool Persistent { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Variable, endianess);
			output.WriteValueU64(this.Value, endianess);
			output.WriteValueB32(this.Persistent, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Variable = input.ReadValueU64(endianess);
			this.Value = input.ReadValueU64(endianess);
			this.Persistent = input.ReadValueB32(endianess);
		}
	}
}
