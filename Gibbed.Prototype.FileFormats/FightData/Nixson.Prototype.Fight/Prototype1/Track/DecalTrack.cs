using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Decal)]
	public class DecalTrack : P1Track
	{
		public ulong ShaderName { get; set; }
		public int RandomSelect { get; set; }
		public float RandomOrientation { get; set; }
		public bool ScaleByDamage { get; set; }
		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public string Patch { get; set; }
		public DecalTrack.DecalStoreType DecalStore { get; set; }
		public bool CullOffscreen { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.ShaderName, endianess);
			output.WriteValueS32(this.RandomSelect, endianess);
			output.WriteValueF32(this.RandomOrientation, endianess);
			output.WriteValueB32(this.ScaleByDamage, endianess);
			output.WriteValueF32(this.ScaleX, endianess);
			output.WriteValueF32(this.ScaleY, endianess);
			output.WriteStringAlignedU32(this.Patch, endianess);
			BaseProperty.SerializePropertyEnum<DecalTrack.DecalStoreType>(output, endianess, this.DecalStore);
			output.WriteValueB32(this.CullOffscreen, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ShaderName = input.ReadValueU64(endianess);
			this.RandomSelect = input.ReadValueS32(endianess);
			this.RandomOrientation = input.ReadValueF32(endianess);
			this.ScaleByDamage = input.ReadValueB32(endianess);
			this.ScaleX = input.ReadValueF32(endianess);
			this.ScaleY = input.ReadValueF32(endianess);
			this.Patch = input.ReadStringAlignedU32(endianess);
			this.DecalStore = BaseProperty.DeserializePropertyEnum<DecalTrack.DecalStoreType>(input, endianess);
			this.CullOffscreen = input.ReadValueB32(endianess);
		}
		public enum DecalStoreType : ulong
		{
			General = 1312132684818588012UL,
			Treadmark = 12991007662534441811UL
		}
	}
}
