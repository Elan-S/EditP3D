using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(12710624925526722085UL)]
	public class MotionStateCondition : P1Condition
	{
		public MovementMotionState State { get; set; }
		public ulong Name { get; set; }
		public bool Match { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<MovementMotionState>(output, endianess, this.State);
			output.WriteValueU64(this.Name, endianess);
			output.WriteValueB32(this.Match, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.State = BaseProperty.DeserializePropertyEnum<MovementMotionState>(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.Match = input.ReadValueB32(endianess);
		}
	}
}
