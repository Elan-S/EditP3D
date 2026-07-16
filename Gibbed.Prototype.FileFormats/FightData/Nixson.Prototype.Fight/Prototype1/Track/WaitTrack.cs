using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Wait)]
	public class WaitTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEndMin { get; set; }
		public float TimeEndMax { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEndMin, endianess);
			output.WriteValueF32(this.TimeEndMax, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEndMin = input.ReadValueF32(endianess);
			this.TimeEndMax = input.ReadValueF32(endianess);
		}
	}
}
