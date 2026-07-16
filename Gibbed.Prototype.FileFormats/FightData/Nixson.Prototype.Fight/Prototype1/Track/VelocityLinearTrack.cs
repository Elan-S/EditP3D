using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14057010424998706298UL)]
	public class VelocityLinearTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public OperationType Operation { get; set; }
		public Vector LinearVelocity { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<OperationType>(output, endianess, this.Operation);
			this.LinearVelocity.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Operation = BaseProperty.DeserializePropertyEnum<OperationType>(input, endianess);
			this.LinearVelocity = new Vector(input, endianess);
		}
	}
}
