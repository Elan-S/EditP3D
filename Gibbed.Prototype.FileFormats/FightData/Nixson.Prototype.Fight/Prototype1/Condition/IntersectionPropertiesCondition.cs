using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(14061254130691643100UL)]
	public class IntersectionPropertiesCondition : P1Condition
	{
		public IntersectionPropertiesCondition.IntersectionType Intersection { get; set; }
		public ColliderType Colliders { get; set; }
		public bool Match { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<IntersectionPropertiesCondition.IntersectionType>(output, endianess, this.Intersection);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.Colliders);
			output.WriteValueB32(this.Match, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Intersection = BaseProperty.DeserializePropertyEnum<IntersectionPropertiesCondition.IntersectionType>(input, endianess);
			this.Colliders = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.Match = input.ReadValueB32(endianess);
		}
		public enum IntersectionType : ulong
		{
			ColliderType = 7209654337809844792UL,
			CollideWithType = 6245265708753804894UL
		}
	}
}
