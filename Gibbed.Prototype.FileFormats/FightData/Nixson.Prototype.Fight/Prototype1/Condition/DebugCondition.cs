using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.Debug)]
	public class DebugCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public DebugCondition.ReleaseModeType Mode { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			BaseProperty.SerializePropertyEnum<DebugCondition.ReleaseModeType>(output, endianess, this.Mode);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Mode = BaseProperty.DeserializePropertyEnum<DebugCondition.ReleaseModeType>(input, endianess);
		}
		public enum ReleaseModeType : ulong
		{
			Debug = 14087970539477689809UL,
			Tune = 11779051480164391926UL,
			Release = 15988309783157748103UL
		}
	}
}
