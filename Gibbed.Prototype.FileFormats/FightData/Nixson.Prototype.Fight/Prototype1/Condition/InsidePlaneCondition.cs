using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.InsidePlane)]
	public class InsidePlaneCondition : P1Condition
	{
		public ulong GrabSlot { get; set; }
		public ulong Joint { get; set; }
		public bool Inside { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Orientation { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueB32(this.Inside, endianess);
			this.Offset.Serialize(output, endianess);
			this.Orientation.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Inside = input.ReadValueB32(endianess);
			this.Offset.Deserialize(input, endianess);
			this.Orientation.Deserialize(input, endianess);
		}
	}
}
