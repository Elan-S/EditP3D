using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(9567076969822214680UL)]
	public class TargetHasLoFCondition : P1Condition
	{
		public float TimeTolerance { get; set; }
		public float DistanceTolerance { get; set; }
		public CollisionFlagsType CollisionFlags { get; set; }
		public ColliderType CollideWithType { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public float Radius { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeTolerance, endianess);
			output.WriteValueF32(this.DistanceTolerance, endianess);
			BaseProperty.SerializePropertyEnum<CollisionFlagsType>(output, endianess, this.CollisionFlags);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWithType);
			this.Offset.Serialize(output, endianess);
			output.WriteValueF32(this.Radius, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeTolerance = input.ReadValueF32(endianess);
			this.DistanceTolerance = input.ReadValueF32(endianess);
			this.CollisionFlags = BaseProperty.DeserializePropertyEnum<CollisionFlagsType>(input, endianess);
			this.CollideWithType = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.Offset = new Vector(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
		}
	}
}
