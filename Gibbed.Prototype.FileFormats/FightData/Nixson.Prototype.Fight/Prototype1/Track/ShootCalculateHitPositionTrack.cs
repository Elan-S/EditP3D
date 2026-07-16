using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ShootCalculateHitPosition)]
	public class ShootCalculateHitPositionTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool ApplyOnParent { get; set; }
		public ulong GrabSlot { get; set; }
		public ulong WeaponEntry { get; set; }
		public ulong Shooter { get; set; }
		public ulong AmmoProfile { get; set; }
		public ulong UserProfile { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.ApplyOnParent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.WeaponEntry, endianess);
			output.WriteValueU64(this.Shooter, endianess);
			output.WriteValueU64(this.AmmoProfile, endianess);
			output.WriteValueU64(this.UserProfile, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ApplyOnParent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.WeaponEntry = input.ReadValueU64(endianess);
			this.Shooter = input.ReadValueU64(endianess);
			this.AmmoProfile = input.ReadValueU64(endianess);
			this.UserProfile = input.ReadValueU64(endianess);
		}
	}
}
