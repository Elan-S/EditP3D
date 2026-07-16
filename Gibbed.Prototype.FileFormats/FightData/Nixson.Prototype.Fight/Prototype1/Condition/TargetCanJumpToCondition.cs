using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.TargetCanJumpTo)]
	public class TargetCanJumpToCondition : P1Condition
	{
		public float LaunchAngle { get; set; }
		public float MinLaunchAngle { get; set; }
		public float Gravity { get; set; }
		public float MaxInitialVelocity { get; set; }
		public bool UseStoredValues { get; set; }
		public float Margin { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.LaunchAngle, endianess);
			output.WriteValueF32(this.MinLaunchAngle, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.MaxInitialVelocity, endianess);
			output.WriteValueB32(this.UseStoredValues, endianess);
			output.WriteValueF32(this.Margin, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.LaunchAngle = input.ReadValueF32(endianess);
			this.MinLaunchAngle = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.MaxInitialVelocity = input.ReadValueF32(endianess);
			this.UseStoredValues = input.ReadValueB32(endianess);
			this.Margin = input.ReadValueF32(endianess);
		}
	}
}
