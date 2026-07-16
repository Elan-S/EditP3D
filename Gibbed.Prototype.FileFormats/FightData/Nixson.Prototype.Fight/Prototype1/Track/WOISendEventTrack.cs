using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11703066415371095195UL)]
	public class WOISendEventTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool ToScenario { get; set; }
		public bool ToFrontend { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.ToScenario, endianess);
			output.WriteValueB32(this.ToFrontend, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ToScenario = input.ReadValueB32(endianess);
			this.ToFrontend = input.ReadValueB32(endianess);
		}
	}
}
