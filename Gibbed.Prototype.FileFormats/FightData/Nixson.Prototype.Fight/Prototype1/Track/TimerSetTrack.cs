using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10558817890755567709UL)]
	public class TimerSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Timer { get; set; }
		public float Time { get; set; }
		public bool Incrementing { get; set; }
		public float RandomRange { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Timer, endianess);
			output.WriteValueF32(this.Time, endianess);
			output.WriteValueB32(this.Incrementing, endianess);
			output.WriteValueF32(this.RandomRange, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Timer = input.ReadValueU64(endianess);
			this.Time = input.ReadValueF32(endianess);
			this.Incrementing = input.ReadValueB32(endianess);
			this.RandomRange = input.ReadValueF32(endianess);
		}
	}
}
