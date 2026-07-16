using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.LocoInput)]
	public class LocoInputCondition : P1Condition
	{
		public LocoInputCondition.InputMotionType InputType { get; set; }
		public CompareOperator CompareTime { get; set; }
		public float Time { get; set; }
		public CompareOperator CompareValue { get; set; }
		public float Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<LocoInputCondition.InputMotionType>(output, endianess, this.InputType);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Time, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareValue);
			output.WriteValueF32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.InputType = BaseProperty.DeserializePropertyEnum<LocoInputCondition.InputMotionType>(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Time = input.ReadValueF32(endianess);
			this.CompareValue = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Value = input.ReadValueF32(endianess);
		}
		public enum InputMotionType : ulong
		{
			LinearMotion = 4848880419476874243UL,
			Steering = 16854725670608644405UL
		}
	}
}
