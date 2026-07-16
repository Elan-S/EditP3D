using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(18040419468861228306UL)]
	public class AimingAtCondition : P1Condition
	{
		public ulong Joint { get; set; }
		public float Tolerance { get; set; }
		public bool Is3D { get; set; }
		public Vector Heading { get; set; } = new Vector();
		public Vector Offset { get; set; } = new Vector();
		public LookAtTarget Where { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueF32(this.Tolerance, endianess);
			output.WriteValueB32(this.Is3D, endianess);
			this.Heading.Serialize(output, endianess);
			this.Offset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<LookAtTarget>(output, endianess, this.Where);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Tolerance = input.ReadValueF32(endianess);
			this.Is3D = input.ReadValueB32(endianess);
			this.Heading.Deserialize(input, endianess);
			this.Offset.Deserialize(input, endianess);
			this.Where = BaseProperty.DeserializePropertyEnum<LookAtTarget>(input, endianess);
		}
	}
}
