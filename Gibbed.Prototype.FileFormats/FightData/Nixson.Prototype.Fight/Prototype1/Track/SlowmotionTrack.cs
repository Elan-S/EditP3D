using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.SlowMotion)]
	public class SlowmotionTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public float SimulationRate { get; set; }
		public float FadeInTime { get; set; }
		public float SimulationDuration { get; set; }
		public float FadeOutTime { get; set; }
		public bool Realtime { get; set; }
		public bool AbortOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			output.WriteValueF32(this.SimulationRate, endianess);
			output.WriteValueF32(this.FadeInTime, endianess);
			output.WriteValueF32(this.SimulationDuration, endianess);
			output.WriteValueF32(this.FadeOutTime, endianess);
			output.WriteValueB32(this.Realtime, endianess);
			output.WriteValueB32(this.AbortOnEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
			this.SimulationRate = input.ReadValueF32(endianess);
			this.FadeInTime = input.ReadValueF32(endianess);
			this.SimulationDuration = input.ReadValueF32(endianess);
			this.FadeOutTime = input.ReadValueF32(endianess);
			this.Realtime = input.ReadValueB32(endianess);
			this.AbortOnEnd = input.ReadValueB32(endianess);
		}
	}
}
