using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(ConditionHash.SpatialQuery)]
	public class SpatialQueryCondition : P1Condition
	{
		public SpatialQueryCondition.WhatType What { get; set; }
		public ulong WhatData { get; set; }
		public SpatialQueryCondition.WhenType Where { get; set; }
		public float MaxDistance { get; set; }
		public bool ExcludeSelf { get; set; }
		public CompareOperator Compare { get; set; }
		public int Inside { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<SpatialQueryCondition.WhatType>(output, endianess, this.What);
			output.WriteValueU64(this.WhatData, endianess);
			BaseProperty.SerializePropertyEnum<SpatialQueryCondition.WhenType>(output, endianess, this.Where);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueB32(this.ExcludeSelf, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueS32(this.Inside, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.What = BaseProperty.DeserializePropertyEnum<SpatialQueryCondition.WhatType>(input, endianess);
			this.WhatData = input.ReadValueU64(endianess);
			this.Where = BaseProperty.DeserializePropertyEnum<SpatialQueryCondition.WhenType>(input, endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.ExcludeSelf = input.ReadValueB32(endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.Inside = input.ReadValueS32(endianess);
		}
		public enum WhatType : ulong
		{
			Faction = 1292973792857699032UL,
			FactionMilitary = 15591361829469292792UL,
			FactionInfected = 8190877763150242633UL,
			Allies = 18219311826121095490UL,
			Enemies = 6865748069480709712UL,
			Classname = 15041496124380207515UL
		}
		public enum WhenType : ulong
		{
			MyPosition = 284889925761261031UL,
			TargetPosition = 9047280947883197886UL
		}
	}
}
