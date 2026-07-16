using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SimpleMessage)]
	public class SimpleMessageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public SimpleMessageTrack.MessageType Message { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<SimpleMessageTrack.MessageType>(output, endianess, this.Message);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Message = BaseProperty.DeserializePropertyEnum<SimpleMessageTrack.MessageType>(input, endianess);
		}
		public enum MessageType : ulong
		{
			None = 22018610510307286UL,
			Reset = 5832977143813391573UL,
			ShowDebug = 6867926323523701038UL,
			HideDebug = 14783914829641130149UL,
			StartAnimation = 10120730374998231438UL,
			StopAnimation = 9047650238404902018UL,
			ForceQueryTree = 5275318950478620811UL
		}
	}
}
