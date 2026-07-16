using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11062236574041393772UL)]
	public class ChargeSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ChargeType Type { get; set; }
		public float Charge { get; set; }
		public float RandomRange { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<ChargeType>(output, endianess, this.Type);
			output.WriteValueF32(this.Charge, endianess);
			output.WriteValueF32(this.RandomRange, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<ChargeType>(input, endianess);
			this.Charge = input.ReadValueF32(endianess);
			this.RandomRange = input.ReadValueF32(endianess);
		}
	}
}
