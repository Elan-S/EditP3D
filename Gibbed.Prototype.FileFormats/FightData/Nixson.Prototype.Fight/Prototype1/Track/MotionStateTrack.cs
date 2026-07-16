using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12710624925526722085UL)]
	public class MotionStateTrack : P1Track
	{
		public float BeginTime { get; set; }
		public MovementMotionState State { get; set; }
		public ulong Name { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			BaseProperty.SerializePropertyEnum<MovementMotionState>(output, endianess, this.State);
			output.WriteValueU64(this.Name, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.State = BaseProperty.DeserializePropertyEnum<MovementMotionState>(input, endianess);
			this.Name = input.ReadValueU64(endianess);
		}
	}
}
