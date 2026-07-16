using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13094784025693200594UL)]
	public class RequestTokenTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public RequestTokenTrack.RequestTokenType Type { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<RequestTokenTrack.RequestTokenType>(output, endianess, this.Type);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<RequestTokenTrack.RequestTokenType>(input, endianess);
		}
		public enum RequestTokenType : ulong
		{
			Hunter = 6865395180375055270UL,
			FireClearRequest = 7926183158331532086UL
		}
	}
}
