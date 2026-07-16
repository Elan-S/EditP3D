using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(17302489414074946182UL)]
	public class SetRotationAttributeTrack : P1Track
	{
		public Vector RotationAxis { get; set; } = new Vector();
		public float RotationDegrees { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			this.RotationAxis.Serialize(output, endianess);
			output.WriteValueF32(this.RotationDegrees, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.RotationAxis.Deserialize(input, endianess);
			this.RotationDegrees = input.ReadValueF32(endianess);
		}
	}
}
