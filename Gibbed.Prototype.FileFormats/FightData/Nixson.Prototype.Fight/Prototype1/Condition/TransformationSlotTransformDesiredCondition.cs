using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.TransformationSlotTransformDesired)]
	public class TransformationSlotTransformDesiredCondition : P1Condition
	{
		public ulong SlotName { get; set; }
		public bool TransformDesired { get; set; }
		public bool Loaded { get; set; }
		public bool Loading { get; set; }
		public bool MatchName { get; set; }
		public ulong DesiredName { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.SlotName, endianess);
			output.WriteValueB32(this.TransformDesired, endianess);
			output.WriteValueB32(this.Loaded, endianess);
			output.WriteValueB32(this.Loading, endianess);
			output.WriteValueB32(this.MatchName, endianess);
			output.WriteValueU64(this.DesiredName, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.SlotName = input.ReadValueU64(endianess);
			this.TransformDesired = input.ReadValueB32(endianess);
			this.Loaded = input.ReadValueB32(endianess);
			this.Loading = input.ReadValueB32(endianess);
			this.MatchName = input.ReadValueB32(endianess);
			this.DesiredName = input.ReadValueU64(endianess);
		}
	}
}
