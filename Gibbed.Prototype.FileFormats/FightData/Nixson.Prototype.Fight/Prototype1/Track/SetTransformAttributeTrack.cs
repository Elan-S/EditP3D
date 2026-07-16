using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SetTransformAttribute)]
	public class SetTransformAttributeTrack : P1Track
	{
		public Vector Transform { get; set; } = new Vector();
		public Vector RotationEulerAnglesXYZ { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			this.Transform.Serialize(output, endianess);
			this.RotationEulerAnglesXYZ.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Transform.Deserialize(input, endianess);
			this.RotationEulerAnglesXYZ.Deserialize(input, endianess);
		}
	}
}
