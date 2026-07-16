using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.CameraAvatarSelect)]
	public class CameraAvatarSelectTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public CameraType CameraType { get; set; }
		public bool Parent { get; set; }
		public ulong GrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<CameraType>(output, endianess, this.CameraType);
			output.WriteValueB32(this.Parent, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.CameraType = BaseProperty.DeserializePropertyEnum<CameraType>(input, endianess);
			this.Parent = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
	}
}
