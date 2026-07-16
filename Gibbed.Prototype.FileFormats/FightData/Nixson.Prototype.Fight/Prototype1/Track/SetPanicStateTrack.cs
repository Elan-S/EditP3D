using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13244813436406909892UL)]
	public class SetPanicStateTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public SetPanicStateTrack.PanicEnumType PanicType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<SetPanicStateTrack.PanicEnumType>(output, endianess, this.PanicType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.PanicType = BaseProperty.DeserializePropertyEnum<SetPanicStateTrack.PanicEnumType>(input, endianess);
		}
		public enum PanicEnumType : ulong
		{
			Normal = 1737215327242219437UL,
			Panic = 7983194175341262185UL,
			EBrake = 10938921865974813017UL
		}
	}
}
