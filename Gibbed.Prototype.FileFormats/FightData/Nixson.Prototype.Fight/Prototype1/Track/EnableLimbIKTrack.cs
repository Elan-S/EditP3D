using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12234078638484311505UL)]
	public class EnableLimbIKTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public EnableLimbIKTrack.LimbIKOnBeginAction ActionOnBegin { get; set; }
		public EnableLimbIKTrack.LimbIKOnEndAction ActionOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<EnableLimbIKTrack.LimbIKOnBeginAction>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<EnableLimbIKTrack.LimbIKOnEndAction>(output, endianess, this.ActionOnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<EnableLimbIKTrack.LimbIKOnBeginAction>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<EnableLimbIKTrack.LimbIKOnEndAction>(input, endianess);
		}
		public enum LimbIKOnBeginAction : ulong
		{
			DoNothing = 3876518407870578744UL,
			EnableLimbIK = 8673583303231311789UL,
			DisableLimbIK = 12982675630842063176UL
		}
		public enum LimbIKOnEndAction : ulong
		{
			RestorePrevious = 6206664339066247152UL,
			EnableLimbIK = 8673583303231311789UL,
			DisableLimbIK = 12982675630842063176UL
		}
	}
}
