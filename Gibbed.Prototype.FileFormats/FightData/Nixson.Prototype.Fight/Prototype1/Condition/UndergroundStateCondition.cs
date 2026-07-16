using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(17879887860711341646UL)]
	public class UndergroundStateCondition : P1Condition
	{
		public UndergroundStateCondition.UndergrounStateType State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<UndergroundStateCondition.UndergrounStateType>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.State = BaseProperty.DeserializePropertyEnum<UndergroundStateCondition.UndergrounStateType>(input, endianess);
		}
		public enum UndergrounStateType : ulong
		{
			Outside = 3287350578625685413UL,
			Underground = 3391944239439239047UL,
			ComingOut = 16788696097060275829UL,
			GoingUnder = 3665617697216476576UL
		}
	}
}
