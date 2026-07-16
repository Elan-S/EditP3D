using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10873567982309138281UL)]
	public class SetFireRateTimerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Timer { get; set; }
		public bool Incrementing { get; set; }
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
			output.WriteValueU64(this.Timer, endianess);
			output.WriteValueB32(this.Incrementing, endianess);
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
			this.Timer = input.ReadValueU64(endianess);
			this.Incrementing = input.ReadValueB32(endianess);
			this.ApplyOnParent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.WeaponEntry = input.ReadValueU64(endianess);
			this.Shooter = input.ReadValueU64(endianess);
			this.AmmoProfile = input.ReadValueU64(endianess);
			this.UserProfile = input.ReadValueU64(endianess);
		}
	}
}
