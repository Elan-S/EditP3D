using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(9504487945226923096UL)]
	public class DetectObbCollisionCondition : P1Condition
	{
		public Collidables CollideWith { get; set; }
		public bool Match { get; set; }
		public ulong Joint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Orientation { get; set; } = new Vector();
		public Vector Extents { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyBitfield<Collidables>(output, endianess, this.CollideWith);
			output.WriteValueB32(this.Match, endianess);
			output.WriteValueU64(this.Joint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Orientation.Serialize(output, endianess);
			this.Extents.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<Collidables>(input, endianess);
			this.Match = input.ReadValueB32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Offset.Deserialize(input, endianess);
			this.Orientation.Deserialize(input, endianess);
			this.Extents.Deserialize(input, endianess);
		}
	}
}
