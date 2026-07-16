using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.Mission)]
	public class MissionCondition : P1Condition
	{
		public int Episode { get; set; }
		public int Mission { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueS32(this.Episode, endianess);
			output.WriteValueS32(this.Mission, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Episode = input.ReadValueS32(endianess);
			this.Mission = input.ReadValueS32(endianess);
		}
	}
}
