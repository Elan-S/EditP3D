using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13307285420716760328UL)]
	public class DetachObjectTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong ObjectToDetach { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.ObjectToDetach, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ObjectToDetach = input.ReadValueU64(endianess);
		}
	}
}
