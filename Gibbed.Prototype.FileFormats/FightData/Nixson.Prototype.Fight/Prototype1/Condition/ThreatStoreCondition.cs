using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.ThreatStore)]
	public class ThreatStoreCondition : P1Condition
	{
		public ulong Name { get; set; }
		public ThreatStoreCondition.ThreatObjectType ThreatObject { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Name, endianess);
			BaseProperty.SerializePropertyEnum<ThreatStoreCondition.ThreatObjectType>(output, endianess, this.ThreatObject);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.ThreatObject = BaseProperty.DeserializePropertyEnum<ThreatStoreCondition.ThreatObjectType>(input, endianess);
		}
		public enum ThreatObjectType : ulong
		{
			None = 31052086116886518UL,
			Thrown = 12170483751774391586UL,
			Giver = 7304932156285466995UL
		}
	}
}
