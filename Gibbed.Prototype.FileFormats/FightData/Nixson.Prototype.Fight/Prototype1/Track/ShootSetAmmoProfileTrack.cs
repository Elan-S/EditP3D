using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ShootSetAmmoProfile)]
	public class ShootSetAmmoProfileTrack : P1Track
	{
		public bool ApplyOnParent { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong WeaponEntry { get; set; }
		public ulong ProfileName { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.ApplyOnParent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.WeaponEntry, endianess);
			output.WriteValueU64(this.ProfileName, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ApplyOnParent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.WeaponEntry = input.ReadValueU64(endianess);
			this.ProfileName = input.ReadValueU64(endianess);
		}
	}
}
