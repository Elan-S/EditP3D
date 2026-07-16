using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.CameraShakeRequest)]
	public class CameraShakeRequestTrack : P1Track
	{
		public float BeginTime { get; set; }
		public CameraShakeRequestTrack.CameraShakeSize Size { get; set; }
		public CameraShakeRequestTrack.CameraShakeSize Range { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			BaseProperty.SerializePropertyEnum<CameraShakeRequestTrack.CameraShakeSize>(output, endianess, this.Size);
			BaseProperty.SerializePropertyEnum<CameraShakeRequestTrack.CameraShakeSize>(output, endianess, this.Range);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.Size = BaseProperty.DeserializePropertyEnum<CameraShakeRequestTrack.CameraShakeSize>(input, endianess);
			this.Range = BaseProperty.DeserializePropertyEnum<CameraShakeRequestTrack.CameraShakeSize>(input, endianess);
		}
		public enum CameraShakeSize : ulong
		{
			Small = 8156211258749108097UL,
			Medium = 17784764271712241149UL,
			Large = 7699058135646415813UL
		}
	}
}
