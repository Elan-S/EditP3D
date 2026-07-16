using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.CanStrafeRun)]
	public class CanStrafeRunCondition : P1Condition
	{
		public float TimeTolerance { get; set; }
		public float DistanceTolerance { get; set; }
		public int MinDelta { get; set; }
		public int MaxDelta { get; set; }
		public bool FarthestPossible { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeTolerance, endianess);
			output.WriteValueF32(this.DistanceTolerance, endianess);
			output.WriteValueS32(this.MinDelta, endianess);
			output.WriteValueS32(this.MaxDelta, endianess);
			output.WriteValueB32(this.FarthestPossible, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeTolerance = input.ReadValueF32(endianess);
			this.DistanceTolerance = input.ReadValueF32(endianess);
			this.MinDelta = input.ReadValueS32(endianess);
			this.MaxDelta = input.ReadValueS32(endianess);
			this.FarthestPossible = input.ReadValueB32(endianess);
		}
	}
}
