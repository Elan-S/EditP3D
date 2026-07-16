using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11640315932166162448UL)]
	public class GroundDistanceCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public float Distance { get; set; }
		public float Radius { get; set; }
		public bool GroundUnit { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Distance, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueB32(this.GroundUnit, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.GroundUnit = input.ReadValueB32(endianess);
		}
	}
}
