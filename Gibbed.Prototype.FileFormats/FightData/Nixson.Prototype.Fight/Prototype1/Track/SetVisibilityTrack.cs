using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.SetVisibility)]
	public class SetVisibilityTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public SetVisibilityTrack.VisibilityAction ActionOnBegin { get; set; }
		public SetVisibilityTrack.VisibilityAction ActionOnEnd { get; set; }
		public bool ApplyToGrabSlots { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<SetVisibilityTrack.VisibilityAction>(output, endianess, this.ActionOnBegin);
			BaseProperty.SerializePropertyEnum<SetVisibilityTrack.VisibilityAction>(output, endianess, this.ActionOnEnd);
			output.WriteValueB32(this.ApplyToGrabSlots, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ActionOnBegin = BaseProperty.DeserializePropertyEnum<SetVisibilityTrack.VisibilityAction>(input, endianess);
			this.ActionOnEnd = BaseProperty.DeserializePropertyEnum<SetVisibilityTrack.VisibilityAction>(input, endianess);
			this.ApplyToGrabSlots = input.ReadValueB32(endianess);
		}
		public enum VisibilityAction : ulong
		{
			Hide = 20324808014569680UL,
			Show = 23429415473539035UL,
			DoNothingOnEnd = 16390556839076977012UL
		}
	}
}
