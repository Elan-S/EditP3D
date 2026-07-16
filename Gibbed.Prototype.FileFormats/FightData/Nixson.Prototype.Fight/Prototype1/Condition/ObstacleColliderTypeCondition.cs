using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(18429031514486453107UL)]
	public class ObstacleColliderTypeCondition : P1Condition
	{
		public ObstacleColliderTypeCondition.ContainType Compare { get; set; }
		public ColliderType ColliderType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ObstacleColliderTypeCondition.ContainType>(output, endianess, this.Compare);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.ColliderType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<ObstacleColliderTypeCondition.ContainType>(input, endianess);
			this.ColliderType = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
		}
		public enum ContainType : ulong
		{
			Contains = 6634170926787241387UL,
			DoesNotContain = 13301905139068269446UL
		}
	}
}
