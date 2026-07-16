using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.PadRumbleSet)]
	public class PadRumbleSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public PadRumbleSetTrack.MotorType Motor { get; set; }
		public float Scale { get; set; }
		public float FadeInTime { get; set; }
		public float HoldTime { get; set; }
		public float FadeOutTime { get; set; }
		public bool UseObjectPos { get; set; }
		public float FullIntensityRadius { get; set; }
		public float FallOffRadius { get; set; }
		public bool CancelOnTrackEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<PadRumbleSetTrack.MotorType>(output, endianess, this.Motor);
			output.WriteValueF32(this.Scale, endianess);
			output.WriteValueF32(this.FadeInTime, endianess);
			output.WriteValueF32(this.HoldTime, endianess);
			output.WriteValueF32(this.FadeOutTime, endianess);
			output.WriteValueB32(this.UseObjectPos, endianess);
			output.WriteValueF32(this.FullIntensityRadius, endianess);
			output.WriteValueF32(this.FallOffRadius, endianess);
			output.WriteValueB32(this.CancelOnTrackEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Motor = BaseProperty.DeserializePropertyEnum<PadRumbleSetTrack.MotorType>(input, endianess);
			this.Scale = input.ReadValueF32(endianess);
			this.FadeInTime = input.ReadValueF32(endianess);
			this.HoldTime = input.ReadValueF32(endianess);
			this.FadeOutTime = input.ReadValueF32(endianess);
			this.UseObjectPos = input.ReadValueB32(endianess);
			this.FullIntensityRadius = input.ReadValueF32(endianess);
			this.FallOffRadius = input.ReadValueF32(endianess);
			this.CancelOnTrackEnd = input.ReadValueB32(endianess);
		}
		public enum MotorType : ulong
		{
			Primary = 1337333749093012630UL,
			Secondary = 17176159389396420574UL
		}
	}
}
