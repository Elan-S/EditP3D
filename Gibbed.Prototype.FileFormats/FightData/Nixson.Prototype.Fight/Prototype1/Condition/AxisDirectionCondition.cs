using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(15149230753062226474UL)]
	public class AxisDirectionCondition : P1Condition
	{
		public Axis Axis { get; set; }
		public float Direction { get; set; }
		public float Arc { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<Axis>(output, endianess, this.Axis);
			output.WriteValueF32(this.Direction, endianess);
			output.WriteValueF32(this.Arc, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Axis = BaseProperty.DeserializePropertyEnum<Axis>(input, endianess);
			this.Direction = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
		}
	}
}
