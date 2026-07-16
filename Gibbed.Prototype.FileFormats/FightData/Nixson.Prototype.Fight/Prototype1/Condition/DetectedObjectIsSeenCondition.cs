using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.DetectedObjectIsSeen)]
	public class DetectedObjectIsSeenCondition : P1Condition
	{
		public bool Seen { get; set; }
		public float MaxDistanceFactor { get; set; }
		public float MaxAngle { get; set; }
		public bool OnlyMilitary { get; set; }
		public bool ExcludeIfCantReport { get; set; }
		public float MaxDistance { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Seen, endianess);
			output.WriteValueF32(this.MaxDistanceFactor, endianess);
			output.WriteValueF32(this.MaxAngle, endianess);
			output.WriteValueB32(this.OnlyMilitary, endianess);
			output.WriteValueB32(this.ExcludeIfCantReport, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Seen = input.ReadValueB32(endianess);
			this.MaxDistanceFactor = input.ReadValueF32(endianess);
			this.MaxAngle = input.ReadValueF32(endianess);
			this.OnlyMilitary = input.ReadValueB32(endianess);
			this.ExcludeIfCantReport = input.ReadValueB32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
		}
	}
}
