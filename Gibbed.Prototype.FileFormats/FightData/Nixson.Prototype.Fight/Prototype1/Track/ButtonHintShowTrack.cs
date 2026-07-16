using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10026379336060687776UL)]
	public class ButtonHintShowTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public InputButton Button { get; set; }
		public ButtonHintShowTrack.EnumPressType PressType { get; set; }
		public string DisplayMessage { get; set; }
		public ulong ScenarioMessage { get; set; }
		public int Priority { get; set; }
		public bool Persistent { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<InputButton>(output, endianess, this.Button);
			BaseProperty.SerializePropertyEnum<ButtonHintShowTrack.EnumPressType>(output, endianess, this.PressType);
			output.WriteStringAlignedU32(this.DisplayMessage, endianess);
			output.WriteValueU64(this.ScenarioMessage, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueB32(this.Persistent, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Button = BaseProperty.DeserializePropertyEnum<InputButton>(input, endianess);
			this.PressType = BaseProperty.DeserializePropertyEnum<ButtonHintShowTrack.EnumPressType>(input, endianess);
			this.DisplayMessage = input.ReadStringAlignedU32(endianess);
			this.ScenarioMessage = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.Persistent = input.ReadValueB32(endianess);
		}
		public enum EnumPressType : ulong
		{
			TAP = 361475483139UL,
			HOLD = 20324971539363327UL,
			MASH = 21735858257907179UL
		}
	}
}
