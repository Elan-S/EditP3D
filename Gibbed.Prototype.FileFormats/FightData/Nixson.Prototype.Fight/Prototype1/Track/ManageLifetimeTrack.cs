using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ManageLifetime)]
	public class ManageLifetimeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool ManageLifetime { get; set; }
		public bool AttachedToParent { get; set; }
		public ulong GrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.ManageLifetime, endianess);
			output.WriteValueB32(this.AttachedToParent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ManageLifetime = input.ReadValueB32(endianess);
			this.AttachedToParent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
	}
}
