using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(16161502657775323693UL)]
	public class DevastatorTargetGroundSpikeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Template { get; set; }
		public int Parameter { get; set; }
		public bool Random { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Template, endianess);
			output.WriteValueS32(this.Parameter, endianess);
			output.WriteValueB32(this.Random, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Template = input.ReadValueU64(endianess);
			this.Parameter = input.ReadValueS32(endianess);
			this.Random = input.ReadValueB32(endianess);
		}
	}
}
