using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.AI)]
	[KnownTrack(TrackHash.Aim)]
	public class AimTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public LookAtTarget Where { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public bool ForceWorldAngles { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<LookAtTarget>(output, endianess, this.Where);
			this.Offset.Serialize(output, endianess);
			output.WriteValueB32(this.ForceWorldAngles, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Where = BaseProperty.DeserializePropertyEnum<LookAtTarget>(input, endianess);
			this.Offset = new Vector(input, endianess);
			this.ForceWorldAngles = input.ReadValueB32(endianess);
		}
	}
}
