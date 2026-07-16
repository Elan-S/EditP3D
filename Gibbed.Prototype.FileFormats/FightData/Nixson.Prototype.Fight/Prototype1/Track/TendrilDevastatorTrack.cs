using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13134442124503615343UL)]
	public class TendrilDevastatorTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Distance { get; set; }
		public BranchReference TargetBranch { get; set; } = new BranchReference();
		public int TendrilCount { get; set; }
		public float AttackTimeBegin { get; set; }
		public float AttackTimeDuration { get; set; }
		public AttackType AttackType { get; set; }
		public float AttackDamage { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Distance, endianess);
			this.TargetBranch.Serialize(output, endianess);
			output.WriteValueS32(this.TendrilCount, endianess);
			output.WriteValueF32(this.AttackTimeBegin, endianess);
			output.WriteValueF32(this.AttackTimeDuration, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueF32(this.AttackDamage, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Distance = input.ReadValueF32(endianess);
			this.TargetBranch = new BranchReference(input, endianess);
			this.TendrilCount = input.ReadValueS32(endianess);
			this.AttackTimeBegin = input.ReadValueF32(endianess);
			this.AttackTimeDuration = input.ReadValueF32(endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.AttackDamage = input.ReadValueF32(endianess);
		}
	}
}
