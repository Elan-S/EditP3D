using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(12234186364934743959UL)]
	public class PerceptionEventCondition : P1Condition
	{
		public PerceptionEventCondition.PerceptionEventType Event { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<PerceptionEventCondition.PerceptionEventType>(output, endianess, this.Event);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Event = BaseProperty.DeserializePropertyEnum<PerceptionEventCondition.PerceptionEventType>(input, endianess);
		}
		public enum PerceptionEventType : ulong
		{
			Confused = 5636306821393634529UL,
			KilledEnemy = 6285212487246905469UL,
			LostBait = 760992217660282718UL,
			AcquiredBait = 4818839403936553164UL
		}
	}
}
