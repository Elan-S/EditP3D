using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(15935461069344539253UL)]
	public class TargetHasLoFDistanceCondition : P1Condition
	{
		public float TimeTolerance { get; set; }
		public float DistanceTolerance { get; set; }
		public CollisionFlagsType CollisionFlags { get; set; }
		public ColliderType CollideWithTypeLessThanDist { get; set; }
		public ColliderType CollideWithTypeAnyDist { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public CompareOperator Compare { get; set; }
		public float DistanceFromTargetSqr { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeTolerance, endianess);
			output.WriteValueF32(this.DistanceTolerance, endianess);
			BaseProperty.SerializePropertyEnum<CollisionFlagsType>(output, endianess, this.CollisionFlags);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWithTypeLessThanDist);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWithTypeAnyDist);
			this.Offset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueF32(this.DistanceFromTargetSqr, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeTolerance = input.ReadValueF32(endianess);
			this.DistanceTolerance = input.ReadValueF32(endianess);
			this.CollisionFlags = BaseProperty.DeserializePropertyEnum<CollisionFlagsType>(input, endianess);
			this.CollideWithTypeLessThanDist = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.CollideWithTypeAnyDist = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.Offset.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.DistanceFromTargetSqr = input.ReadValueF32(endianess);
		}
	}
}
