using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.FacingAngleTarget)]
	public class FacingAngleTargetCondition : P1Condition
	{
		public FacingAngleTargetCondition.YawPitchType Type { get; set; }
		public CompareOperator CompareTime { get; set; }
		public float Angle { get; set; }
		public bool Signed { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<FacingAngleTargetCondition.YawPitchType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.CompareTime);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueB32(this.Signed, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<FacingAngleTargetCondition.YawPitchType>(input, endianess);
			this.CompareTime = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.Signed = input.ReadValueB32(endianess);
		}
		public enum YawPitchType : ulong
		{
			Yaw = 520688520109UL,
			Pitch = 7985452587274801402UL
		}
	}
}
