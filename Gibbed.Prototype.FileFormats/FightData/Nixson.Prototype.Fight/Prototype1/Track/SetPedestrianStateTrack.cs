using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14345594806722865390UL)]
	public class SetPedestrianStateTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public AIState State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<AIState>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.State = BaseProperty.DeserializePropertyEnum<AIState>(input, endianess);
		}
	}
}
