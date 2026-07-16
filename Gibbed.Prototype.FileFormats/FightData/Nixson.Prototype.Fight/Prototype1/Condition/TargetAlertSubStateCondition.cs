using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(9349876374104536974UL)]
	public class TargetAlertSubStateCondition : P1Condition
	{
		public TargetAlertSubStateCondition.TargetAlertSubStateType SubState { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<TargetAlertSubStateCondition.TargetAlertSubStateType>(output, endianess, this.SubState);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SubState = BaseProperty.DeserializePropertyEnum<TargetAlertSubStateCondition.TargetAlertSubStateType>(input, endianess);
		}
		public enum TargetAlertSubStateType : ulong
		{
			Default = 16487629634260232271UL,
			Disoriented = 7264117317165478180UL,
			NotFooled = 2569352039904682526UL
		}
	}
}
