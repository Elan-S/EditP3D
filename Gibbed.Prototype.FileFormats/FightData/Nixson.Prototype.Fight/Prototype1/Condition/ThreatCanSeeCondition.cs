using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(10353325727153714363UL)]
	public class ThreatCanSeeCondition : P1Condition
	{
		public ulong Name { get; set; }
		public float FOV { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public bool IgnoreY { get; set; }
		public bool GiversPerspective { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.Name, endianess);
			output.WriteValueF32(this.FOV, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueB32(this.IgnoreY, endianess);
			output.WriteValueB32(this.GiversPerspective, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Name = input.ReadValueU64(endianess);
			this.FOV = input.ReadValueF32(endianess);
			this.Direction.Deserialize(input, endianess);
			this.IgnoreY = input.ReadValueB32(endianess);
			this.GiversPerspective = input.ReadValueB32(endianess);
		}
	}
}
