using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10905333969295431526UL)]
	public class SetSecondaryWeaponTypeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public SetSecondaryWeaponTypeTrack.SecondaryWeaponType Type { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<SetSecondaryWeaponTypeTrack.SecondaryWeaponType>(output, endianess, this.Type);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<SetSecondaryWeaponTypeTrack.SecondaryWeaponType>(input, endianess);
		}
		public enum SecondaryWeaponType : ulong
		{
			Gun50mm = 13590193798748521041UL,
			Rocket = 14395127206582377560UL,
			Missile = 13148182856864364872UL,
			Bloodtox = 18124356316935178433UL
		}
	}
}
