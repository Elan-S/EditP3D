using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.EdgeNormal)]
	public class EdgeNormalCondition : P1Condition
	{
		public Vector Normal { get; set; } = new Vector();
		public float Arc { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			this.Normal.Serialize(output, endianess);
			output.WriteValueF32(this.Arc, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Normal = new Vector(input, endianess);
			this.Arc = input.ReadValueF32(endianess);
		}
	}
}
