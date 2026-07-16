using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17255156770509568857UL)]
	public class TeleportTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public bool RestorePosition { get; set; }
		public ulong Object { get; set; }
		public SpaceType Space { get; set; }
		public ulong SpaceJoint { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector Orientation { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueB32(this.RestorePosition, endianess);
			output.WriteValueU64(this.Object, endianess);
			BaseProperty.SerializePropertyEnum<SpaceType>(output, endianess, this.Space);
			output.WriteValueU64(this.SpaceJoint, endianess);
			this.Offset.Serialize(output, endianess);
			this.Orientation.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.RestorePosition = input.ReadValueB32(endianess);
			this.Object = input.ReadValueU64(endianess);
			this.Space = BaseProperty.DeserializePropertyEnum<SpaceType>(input, endianess);
			this.SpaceJoint = input.ReadValueU64(endianess);
			this.Offset.Deserialize(input, endianess);
			this.Orientation.Deserialize(input, endianess);
		}
	}
}
