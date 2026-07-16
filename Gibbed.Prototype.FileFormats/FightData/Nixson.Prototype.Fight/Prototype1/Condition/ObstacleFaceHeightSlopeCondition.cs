using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.ObstacleFaceHeightSlope)]
	public class ObstacleFaceHeightSlopeCondition : P1Condition
	{
		public float FaceHeightMin { get; set; }
		public float HeightMax { get; set; }
		public float SlopeMax { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.FaceHeightMin, endianess);
			output.WriteValueF32(this.HeightMax, endianess);
			output.WriteValueF32(this.SlopeMax, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.FaceHeightMin = input.ReadValueF32(endianess);
			this.HeightMax = input.ReadValueF32(endianess);
			this.SlopeMax = input.ReadValueF32(endianess);
		}
	}
}
