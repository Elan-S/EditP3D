using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11401369732852470972UL)]
	public class SoundBoolValueTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Patch { get; set; }
		public ulong Control { get; set; }
		public bool Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Patch, endianess);
			output.WriteValueU64(this.Control, endianess);
			output.WriteValueB32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Patch = input.ReadValueU64(endianess);
			this.Control = input.ReadValueU64(endianess);
			this.Value = input.ReadValueB32(endianess);
		}
	}
}
