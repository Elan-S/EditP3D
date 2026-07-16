using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.CameraTypeSelect)]
	public class CameraTypeSelectTrack : P1Track
	{
		public float BeginTime { get; set; }
		public CameraType CameraType { get; set; }
		public float CameraBlendTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			BaseProperty.SerializePropertyEnum<CameraType>(output, endianess, this.CameraType);
			output.WriteValueF32(this.CameraBlendTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.CameraType = BaseProperty.DeserializePropertyEnum<CameraType>(input, endianess);
			this.CameraBlendTime = input.ReadValueF32(endianess);
		}
	}
}
