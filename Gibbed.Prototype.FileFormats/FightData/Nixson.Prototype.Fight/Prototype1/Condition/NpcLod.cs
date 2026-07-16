using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(18297938226954427718UL)]
	public class NpcLod : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public NpcLod.LODStatus Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			BaseProperty.SerializePropertyEnum<NpcLod.LODStatus>(output, endianess, this.Value);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Value = BaseProperty.DeserializePropertyEnum<NpcLod.LODStatus>(input, endianess);
		}
		public enum LODStatus : ulong
		{
			Active = 382516289659353610UL,
			Frozen = 9014025788489785814UL,
			LOD2 = 21454155989660813UL
		}
	}
}
