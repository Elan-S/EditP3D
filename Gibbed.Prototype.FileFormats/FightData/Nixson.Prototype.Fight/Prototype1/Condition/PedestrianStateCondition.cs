using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.PedestrianState)]
	public class PedestrianStateCondition : P1Condition
	{
		public bool Match { get; set; }
		public PedestrianStateCondition.PedestrianStateType State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Match, endianess);
			BaseProperty.SerializePropertyEnum<PedestrianStateCondition.PedestrianStateType>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Match = input.ReadValueB32(endianess);
			this.State = BaseProperty.DeserializePropertyEnum<PedestrianStateCondition.PedestrianStateType>(input, endianess);
		}
		public enum PedestrianStateType : ulong
		{
			PedestrianState_Walking = 18075099910908824922UL,
			PedestrianState_Running = 15550110210725836214UL,
			PedestrianState_Crosswalk_Walking = 14570086209907466100UL,
			PedestrianState_Crosswalk_Running = 2630255935707042220UL,
			PedestrianState_Panicked = 16961301976731947450UL,
			PedestrianState_InfectedAreaPanicked = 14471925035603885565UL,
			PedestrianState_Cowering = 3424767917318801581UL,
			PedestrianState_InfectedAreaCowering = 9292131652635289306UL,
			PedestrianState_Standing = 8131735889933293097UL,
			PedestrianState_AtRedLight = 17885541790587772205UL,
			PedestrianState_Blocked = 10269630828599918459UL,
			PedestrianState_Dead = 18390712503526155491UL,
			PedestrianState_SpawnDead = 9421079735691181128UL,
			PedestrianState_PhysicsSimulated = 4477934107992374178UL,
			PedestrianState_Rubbernecking1 = 6499267149839634035UL,
			PedestrianState_Rubbernecking2 = 6499267149839634032UL,
			PedestrianState_JogAvoid = 1711324512440749982UL
		}
	}
}
