using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(12140870524299593779UL)]
	public class HaveGrabTargetCondition : P1Condition
	{
		public bool Have { get; set; }
		public bool UseStoredProperties { get; set; }
		public ColliderType CollideWith { get; set; }
		public ColliderType Ignore { get; set; }
		public ulong ValidGrabbableClass { get; set; }
		public float Range { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Have, endianess);
			output.WriteValueB32(this.UseStoredProperties, endianess);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWith);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.Ignore);
			output.WriteValueU64(this.ValidGrabbableClass, endianess);
			output.WriteValueF32(this.Range, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Have = input.ReadValueB32(endianess);
			this.UseStoredProperties = input.ReadValueB32(endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.Ignore = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.ValidGrabbableClass = input.ReadValueU64(endianess);
			this.Range = input.ReadValueF32(endianess);
		}
	}
}
