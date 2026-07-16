using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(15786763781211051194UL)]
	public class SurfaceAngleCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public float Tolerance { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			this.Direction.Serialize(output, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Direction.Deserialize(input, endianess);
			this.Tolerance = input.ReadValueF32(endianess);
		}
	}
}
