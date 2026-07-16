using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14548838721734496884UL)]
	public class WaitOnCollisionTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ColliderType CollideWith { get; set; }
		public float ImpulseThreshold { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWith);
			output.WriteValueF32(this.ImpulseThreshold, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.ImpulseThreshold = input.ReadValueF32(endianess);
		}
	}
}
