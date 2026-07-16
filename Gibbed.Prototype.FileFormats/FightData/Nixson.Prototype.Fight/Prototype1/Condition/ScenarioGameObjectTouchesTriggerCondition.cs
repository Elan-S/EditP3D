using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownCondition(ConditionHash.GameObjectTouchesTrigger)]
	public class ScenarioGameObjectTouchesTriggerCondition : P1Condition
	{
		public ulong GameObjectNameHash { get; set; }
		public ulong TriggerNameHash { get; set; }
		public ScenarioGameObjectTouchesTriggerCondition.TriggerDirection Direction { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GameObjectNameHash, endianess);
			output.WriteValueU64(this.TriggerNameHash, endianess);
			BaseProperty.SerializePropertyEnum<ScenarioGameObjectTouchesTriggerCondition.TriggerDirection>(output, endianess, this.Direction);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GameObjectNameHash = input.ReadValueU64(endianess);
			this.TriggerNameHash = input.ReadValueU64(endianess);
			this.Direction = BaseProperty.DeserializePropertyEnum<ScenarioGameObjectTouchesTriggerCondition.TriggerDirection>(input, endianess);
		}
		public enum TriggerDirection : ulong
		{
			Enter = 4872555661335756302UL,
			Exit = 19477321536111832UL
		}
	}
}
