using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14202284275051165443UL)]
	public class ScenarioStringEventTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong String { get; set; }
		public bool TutorialOnly { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.String, endianess);
			output.WriteValueB32(this.TutorialOnly, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.String = input.ReadValueU64(endianess);
			this.TutorialOnly = input.ReadValueB32(endianess);
		}
	}
}
