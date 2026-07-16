using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14404407927375413006UL)]
	public class CameraResetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public float BlendTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueF32(this.BlendTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Direction = new Vector(input, endianess);
			this.BlendTime = input.ReadValueF32(endianess);
		}
	}
}
