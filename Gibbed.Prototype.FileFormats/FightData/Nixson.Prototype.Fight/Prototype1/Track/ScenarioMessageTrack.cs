using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Alert)]
	[KnownTrack(10982344578780053135UL)]
	public class ScenarioMessageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ScenarioMessageTrack.MessageType Event { get; set; }
		public bool OnBegin { get; set; }
		public bool OnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<ScenarioMessageTrack.MessageType>(output, endianess, this.Event);
			output.WriteValueB32(this.OnBegin, endianess);
			output.WriteValueB32(this.OnEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Event = BaseProperty.DeserializePropertyEnum<ScenarioMessageTrack.MessageType>(input, endianess);
			this.OnBegin = input.ReadValueB32(endianess);
			this.OnEnd = input.ReadValueB32(endianess);
		}
		public enum MessageType : ulong
		{
			EnterAlert = 1212611220454321848UL,
			LeaveAlert = 14301762854519847273UL,
			CallStrikeTeam = 7659658666603352665UL,
			PlayerEngaged = 9539135133961630246UL
		}
	}
}
