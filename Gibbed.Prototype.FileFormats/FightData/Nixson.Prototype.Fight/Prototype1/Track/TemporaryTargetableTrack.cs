using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.TemporaryTargetable)]
	public class TemporaryTargetableTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public TemporaryTargetableTrack.OnEventType OnBegin { get; set; }
		public TemporaryTargetableTrack.OnEventType OnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<TemporaryTargetableTrack.OnEventType>(output, endianess, this.OnBegin);
			BaseProperty.SerializePropertyEnum<TemporaryTargetableTrack.OnEventType>(output, endianess, this.OnEnd);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.OnBegin = BaseProperty.DeserializePropertyEnum<TemporaryTargetableTrack.OnEventType>(input, endianess);
			this.OnEnd = BaseProperty.DeserializePropertyEnum<TemporaryTargetableTrack.OnEventType>(input, endianess);
		}
		public enum OnEventType : ulong
		{
			DontChange = 7513540739177987366UL,
			Targetable = 686235301512787779UL,
			NonTargetable = 15836497568040073772UL,
			Restore = 17405989608029490314UL
		}
	}
}
