using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.ShootingEvent)]
	public class ShootingEventCondition : P1Condition
	{
		public ShootingEventCondition.ShootingEventType Event { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<ShootingEventCondition.ShootingEventType>(output, endianess, this.Event);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Event = BaseProperty.DeserializePropertyEnum<ShootingEventCondition.ShootingEventType>(input, endianess);
		}
		public enum ShootingEventType : ulong
		{
			ShootBegin = 3674486027934643056UL,
			ShootEnd = 7583024027496011076UL,
			ShotFired = 12142346295941938800UL,
			ClipEmpty = 12678760736155385221UL,
			Discarded = 1339307571350710661UL,
			PickedUp = 16979082672551358635UL
		}
	}
}
