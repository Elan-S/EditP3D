using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.CauseDamage)]
	public class CauseDamageTrack : P1Track
	{
		public float BeginTime { get; set; }
		public bool ForceZeroHealth { get; set; }
		public float Damage { get; set; }
		public AttackType AttackType { get; set; }
		public CauseDamageTrack.DamageOriginatorType Originator { get; set; }
		public ulong GrabSlotName { get; set; }
		public bool IgnoreShield { get; set; }
		public bool SendAlert { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueB32(this.ForceZeroHealth, endianess);
			output.WriteValueF32(this.Damage, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			BaseProperty.SerializePropertyEnum<CauseDamageTrack.DamageOriginatorType>(output, endianess, this.Originator);
			output.WriteValueU64(this.GrabSlotName, endianess);
			output.WriteValueB32(this.IgnoreShield, endianess);
			output.WriteValueB32(this.SendAlert, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.ForceZeroHealth = input.ReadValueB32(endianess);
			this.Damage = input.ReadValueF32(endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.Originator = BaseProperty.DeserializePropertyEnum<CauseDamageTrack.DamageOriginatorType>(input, endianess);
			this.GrabSlotName = input.ReadValueU64(endianess);
			this.IgnoreShield = input.ReadValueB32(endianess);
			this.SendAlert = input.ReadValueB32(endianess);
		}
		public enum DamageOriginatorType : ulong
		{
			HitOriginator = 10079825596818229623UL,
			GameObject = 16917872325796917431UL,
			GrabSlot = 7882620167033900854UL,
			ParentGameObject = 14378065813107577471UL,
			Player = 12290011226446443607UL
		}
	}
}
