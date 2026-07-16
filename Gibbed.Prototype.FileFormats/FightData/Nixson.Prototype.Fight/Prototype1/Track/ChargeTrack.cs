using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(15893711816904805024UL)]
	public class ChargeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ChargeType Type { get; set; }
		public bool ClearOnBegin { get; set; }
		public float FinalCharge { get; set; }
		public float RandomRange { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<ChargeType>(output, endianess, this.Type);
			output.WriteValueB32(this.ClearOnBegin, endianess);
			output.WriteValueF32(this.FinalCharge, endianess);
			output.WriteValueF32(this.RandomRange, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<ChargeType>(input, endianess);
			this.ClearOnBegin = input.ReadValueB32(endianess);
			this.FinalCharge = input.ReadValueF32(endianess);
			this.RandomRange = input.ReadValueF32(endianess);
		}
	}
}
