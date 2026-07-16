using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Death)]
	public class DeathTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong DeadTemplate { get; set; }
		public bool CopyDrawable { get; set; }
		public bool GrabberIsOriginator { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.DeadTemplate, endianess);
			output.WriteValueB32(this.CopyDrawable, endianess);
			output.WriteValueB32(this.GrabberIsOriginator, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.DeadTemplate = input.ReadValueU64(endianess);
			this.CopyDrawable = input.ReadValueB32(endianess);
			this.GrabberIsOriginator = input.ReadValueB32(endianess);
		}
	}
}
