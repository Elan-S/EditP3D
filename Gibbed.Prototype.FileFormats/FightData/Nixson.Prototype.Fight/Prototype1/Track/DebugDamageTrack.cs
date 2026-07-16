using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.DebugDamage)]
	public class DebugDamageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float Distance { get; set; }
		public DamageType DamageType { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public ulong DamageSource { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.Distance, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			output.WriteValueF32(this.ScaleX, endianess);
			output.WriteValueF32(this.ScaleY, endianess);
			output.WriteValueU64(this.DamageSource, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.ScaleX = input.ReadValueF32(endianess);
			this.ScaleY = input.ReadValueF32(endianess);
			this.DamageSource = input.ReadValueU64(endianess);
		}
	}
}
