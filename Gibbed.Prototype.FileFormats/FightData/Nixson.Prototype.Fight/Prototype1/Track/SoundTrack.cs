using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Sound)]
	public class SoundTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong Patch { get; set; }
		public ulong Trigger { get; set; }
		public float VolumeFactor { get; set; }
		public float MinDistance { get; set; }
		public float MaxDistance { get; set; }
		public float MinPitch { get; set; }
		public float MaxPitch { get; set; }
		public float CyclicPhase { get; set; }
		public ulong InstanceName { get; set; }
		public float Frequency { get; set; }
		public bool PolledLifetime { get; set; }
		public SoundTrack.SoundTriggerType TriggerType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.Patch, endianess);
			output.WriteValueU64(this.Trigger, endianess);
			output.WriteValueF32(this.VolumeFactor, endianess);
			output.WriteValueF32(this.MinDistance, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueF32(this.MinPitch, endianess);
			output.WriteValueF32(this.MaxPitch, endianess);
			output.WriteValueF32(this.CyclicPhase, endianess);
			output.WriteValueU64(this.InstanceName, endianess);
			output.WriteValueF32(this.Frequency, endianess);
			output.WriteValueB32(this.PolledLifetime, endianess);
			BaseProperty.SerializePropertyEnum<SoundTrack.SoundTriggerType>(output, endianess, this.TriggerType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Patch = input.ReadValueU64(endianess);
			this.Trigger = input.ReadValueU64(endianess);
			this.VolumeFactor = input.ReadValueF32(endianess);
			this.MinDistance = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.MinPitch = input.ReadValueF32(endianess);
			this.MaxPitch = input.ReadValueF32(endianess);
			this.CyclicPhase = input.ReadValueF32(endianess);
			this.InstanceName = input.ReadValueU64(endianess);
			this.Frequency = input.ReadValueF32(endianess);
			this.PolledLifetime = input.ReadValueB32(endianess);
			this.TriggerType = BaseProperty.DeserializePropertyEnum<SoundTrack.SoundTriggerType>(input, endianess);
		}
		public enum SoundTriggerType : ulong
		{
			Retrigger = 13522664087197083521UL,
			Overlap = 10712345420671996437UL,
			StopExisting = 6266415434618234495UL,
			IgnoreIfPlaying = 3427415984850077271UL
		}
	}
}
