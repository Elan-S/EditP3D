using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.VelocityAngular)]
	public class VelocityAngularTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public CompareOperator Operation { get; set; }
		public Vector AngularVelocity { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Operation);
			this.AngularVelocity.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Operation = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.AngularVelocity.Deserialize(input, endianess);
		}
	}
}
