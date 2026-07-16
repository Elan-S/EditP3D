using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(16772072300208446024UL)]
	public class VariableIntTemporarySetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Variable { get; set; }
		public ulong GrabSlot { get; set; }
		public bool UseGrabber { get; set; }
		public AssigmentOperator OperatorBegin { get; set; }
		public AssigmentOperator OperatorEnd { get; set; }
		public int BeginValue { get; set; }
		public int EndValue { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Variable, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueB32(this.UseGrabber, endianess);
			BaseProperty.SerializePropertyEnum<AssigmentOperator>(output, endianess, this.OperatorBegin);
			BaseProperty.SerializePropertyEnum<AssigmentOperator>(output, endianess, this.OperatorEnd);
			output.WriteValueS32(this.BeginValue, endianess);
			output.WriteValueS32(this.EndValue, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Variable = input.ReadValueU64(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.UseGrabber = input.ReadValueB32(endianess);
			this.OperatorBegin = BaseProperty.DeserializePropertyEnum<AssigmentOperator>(input, endianess);
			this.OperatorEnd = BaseProperty.DeserializePropertyEnum<AssigmentOperator>(input, endianess);
			this.BeginValue = input.ReadValueS32(endianess);
			this.EndValue = input.ReadValueS32(endianess);
		}
	}
}
