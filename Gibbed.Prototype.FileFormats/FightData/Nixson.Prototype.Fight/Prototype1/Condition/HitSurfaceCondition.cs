using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(16977234416335862612UL)]
	public class HitSurfaceCondition : P1Condition
	{
		public HitSurfaceCondition.HitSurfaceType Surface { get; set; }
		public bool HitSurface { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<HitSurfaceCondition.HitSurfaceType>(output, endianess, this.Surface);
			output.WriteValueB32(this.HitSurface, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Surface = BaseProperty.DeserializePropertyEnum<HitSurfaceCondition.HitSurfaceType>(input, endianess);
			this.HitSurface = input.ReadValueB32(endianess);
		}
		public enum HitSurfaceType : ulong
		{
			Ground = 11389985536937095513UL,
			Roof = 23147657255643158UL,
			Wall = 24558595612407808UL
		}
	}
}
