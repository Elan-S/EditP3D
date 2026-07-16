using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(13646844925227524532UL)]
	public class DetectedObjectAngleCondition : P1Condition
	{
		public DetectedObjectAngleCondition.DetectedObjectAngleType Type { get; set; }
		public float Angle { get; set; }
		public float Arc { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<DetectedObjectAngleCondition.DetectedObjectAngleType>(output, endianess, this.Type);
			output.WriteValueF32(this.Angle, endianess);
			output.WriteValueF32(this.Arc, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<DetectedObjectAngleCondition.DetectedObjectAngleType>(input, endianess);
			this.Angle = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
		}
		public enum DetectedObjectAngleType : ulong
		{
			Facing = 10328104250444425114UL,
			Input = 5158950104358209040UL
		}
	}
}
