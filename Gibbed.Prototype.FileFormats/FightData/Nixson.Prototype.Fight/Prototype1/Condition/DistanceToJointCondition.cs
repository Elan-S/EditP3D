using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.DistanceToJoint)]
	public class DistanceToJointCondition : P1Condition
	{
		public ulong Joint { get; set; }
		public ulong GrabSlot { get; set; }
		public CompareOperator Compare { get; set; }
		public float Distance { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.Distance, endianess);
			this.Offset.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.Offset = new Vector(input, endianess);
		}
	}
}
