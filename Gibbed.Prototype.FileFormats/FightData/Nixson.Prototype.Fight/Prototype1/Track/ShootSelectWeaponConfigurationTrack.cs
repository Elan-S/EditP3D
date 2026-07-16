using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ShootSelectWeaponConfiguration)]
	public class ShootSelectWeaponConfigurationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool ApplyOnParent { get; set; }
		public ulong WeaponConfiguration { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.ApplyOnParent, endianess);
			output.WriteValueU64(this.WeaponConfiguration, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.ApplyOnParent = input.ReadValueB32(endianess);
			this.WeaponConfiguration = input.ReadValueU64(endianess);
		}
	}
}
