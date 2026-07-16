using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(16030499714225316867UL)]
	public class NavMeshAreaCondition : P1Condition
	{
		public AllowedAreasFlags AllowedAreas { get; set; }
		public float VerticalTolerance { get; set; }
		public float RayIfNotSupported { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyBitfield<AllowedAreasFlags>(output, endianess, this.AllowedAreas);
			output.WriteValueF32(this.VerticalTolerance, endianess);
			output.WriteValueF32(this.RayIfNotSupported, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.AllowedAreas = BaseProperty.DeserializePropertyBitfield<AllowedAreasFlags>(input, endianess);
			this.VerticalTolerance = input.ReadValueF32(endianess);
			this.RayIfNotSupported = input.ReadValueF32(endianess);
		}
	}
}
