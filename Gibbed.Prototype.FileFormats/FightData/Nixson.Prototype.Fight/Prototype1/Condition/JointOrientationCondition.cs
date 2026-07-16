using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.JointOrientation)]
	public class JointOrientationCondition : P1Condition
	{
		public ulong Joint { get; set; }
		public Vector Facing { get; set; } = new Vector();
		public Vector Up { get; set; } = new Vector();
		public float Threshold { get; set; }
		public bool MatchFacing { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Facing.Serialize(output, endianess);
			this.Up.Serialize(output, endianess);
			output.WriteValueF32(this.Threshold, endianess);
			output.WriteValueB32(this.MatchFacing, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Facing.Deserialize(input, endianess);
			this.Up.Deserialize(input, endianess);
			this.Threshold = input.ReadValueF32(endianess);
			this.MatchFacing = input.ReadValueB32(endianess);
		}
	}
}
