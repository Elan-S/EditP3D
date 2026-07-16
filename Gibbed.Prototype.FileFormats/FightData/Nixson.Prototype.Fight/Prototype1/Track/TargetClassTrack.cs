using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11010705680531599253UL)]
	public class TargetClassTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public TargetClass TargetClass { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.TargetClass);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
		}
	}
}
