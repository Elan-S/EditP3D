using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownCondition(14357145225903926731UL)]
	public class ScenarioGameObjectCondition : P1Condition
	{
		public ScenarioGameObjectSlot GameObjectSlot { get; set; }
		public ulong GameObjectNameHash { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ScenarioGameObjectSlot>(output, endianess, this.GameObjectSlot);
			output.WriteValueU64(this.GameObjectNameHash, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GameObjectSlot = BaseProperty.DeserializePropertyEnum<ScenarioGameObjectSlot>(input, endianess);
			this.GameObjectNameHash = input.ReadValueU64(endianess);
		}
	}
}
