using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Stun)]
	public class StunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public StunTrack.StunActionType OnBegin { get; set; }
		public StunTrack.StunActionType OnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<StunTrack.StunActionType>(output, endianess, this.OnBegin);
			BaseProperty.SerializePropertyEnum<StunTrack.StunActionType>(output, endianess, this.OnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.OnBegin = BaseProperty.DeserializePropertyEnum<StunTrack.StunActionType>(input, endianess);
			this.OnEnd = BaseProperty.DeserializePropertyEnum<StunTrack.StunActionType>(input, endianess);
		}
		public enum StunActionType : ulong
		{
			DoNothing = 3876518407870578744UL,
			RestoreToPrevious = 6206664339066247152UL,
			Stun = 23429501539295808UL,
			Recover = 97766630827740312UL
		}
	}
}
