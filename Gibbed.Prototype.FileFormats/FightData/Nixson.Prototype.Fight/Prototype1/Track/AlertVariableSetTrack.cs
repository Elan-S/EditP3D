using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Alert)]
	[KnownTrack(TrackHash.AlertVariableSet)]
	public class AlertVariableSetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public VariableType Variable { get; set; }
		public float Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<VariableType>(output, endianess, this.Variable);
			output.WriteValueF32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Variable = BaseProperty.DeserializePropertyEnum<VariableType>(input, endianess);
			this.Value = input.ReadValueF32(endianess);
		}
	}
}
