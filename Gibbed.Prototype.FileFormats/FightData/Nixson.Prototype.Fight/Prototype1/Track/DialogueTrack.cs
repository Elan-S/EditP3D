using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(9340234385139842878UL)]
	public class DialogueTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public string CharacterName { get; set; }
		public string EventName { get; set; }
		public string PlayOn { get; set; }
		public float Frequency { get; set; }
		public float InstanceFrequencySensitivity { get; set; }
		public int MinDelay { get; set; }
		public int MaxDelay { get; set; }
		public bool KillOnExit { get; set; }
		public bool IfPlayerCanBeSeen { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteStringAlignedU32(this.CharacterName, endianess);
			output.WriteStringAlignedU32(this.EventName, endianess);
			output.WriteStringAlignedU32(this.PlayOn, endianess);
			output.WriteValueF32(this.Frequency, endianess);
			output.WriteValueF32(this.InstanceFrequencySensitivity, endianess);
			output.WriteValueS32(this.MinDelay, endianess);
			output.WriteValueS32(this.MaxDelay, endianess);
			output.WriteValueB32(this.KillOnExit, endianess);
			output.WriteValueB32(this.IfPlayerCanBeSeen, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.CharacterName = input.ReadStringAlignedU32(endianess);
			this.EventName = input.ReadStringAlignedU32(endianess);
			this.PlayOn = input.ReadStringAlignedU32(endianess);
			this.Frequency = input.ReadValueF32(endianess);
			this.InstanceFrequencySensitivity = input.ReadValueF32(endianess);
			this.MinDelay = input.ReadValueS32(endianess);
			this.MaxDelay = input.ReadValueS32(endianess);
			this.KillOnExit = input.ReadValueB32(endianess);
			this.IfPlayerCanBeSeen = input.ReadValueB32(endianess);
		}
	}
}
