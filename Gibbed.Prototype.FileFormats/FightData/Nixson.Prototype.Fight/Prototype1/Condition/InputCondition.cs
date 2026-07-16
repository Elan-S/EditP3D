using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.Input)]
	public class InputCondition : P1Condition
	{
		public InputButton Button { get; set; }
		public InputButtonState ButtonState { get; set; }
		public CompareOperator CompareTime { get; set; }
		public float Time { get; set; }
		public CompareOperator CompareValue { get; set; }
		public float Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<InputButton>(output, endianess, this.Button);
			BaseProperty.SerializePropertyEnum<InputButtonState>(output, endianess, this.ButtonState);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Time, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareValue);
			output.WriteValueF32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Button = BaseProperty.DeserializePropertyEnum<InputButton>(input, endianess);
			this.ButtonState = BaseProperty.DeserializePropertyEnum<InputButtonState>(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Time = input.ReadValueF32(endianess);
			this.CompareValue = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Value = input.ReadValueF32(endianess);
		}
	}
}
