using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownCondition(13534414905141744190UL)]
	public class TransformationDescriptionNameCondition : P1Condition
	{
		public ScenarioGameObjectSlot Affiliate { get; set; }
		public ulong Name { get; set; }
		public bool IsActive { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ScenarioGameObjectSlot>(output, endianess, this.Affiliate);
			output.WriteValueU64(this.Name, endianess);
			output.WriteValueB32(this.IsActive, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Affiliate = BaseProperty.DeserializePropertyEnum<ScenarioGameObjectSlot>(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.IsActive = input.ReadValueB32(endianess);
		}
	}
}
