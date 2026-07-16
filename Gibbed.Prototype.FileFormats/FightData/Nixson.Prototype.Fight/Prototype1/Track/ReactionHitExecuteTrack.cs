using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.ReactionHitExecute)]
	public class ReactionHitExecuteTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float ReactionTimeMin { get; set; }
		public float ReactionTimeMax { get; set; }
		public float ReactionDistanceMin { get; set; }
		public float ReactionDistanceMax { get; set; }
		public float ReactionHeightMin { get; set; }
		public float ReactionHeightMax { get; set; }
		public Vector ReactionDirection { get; set; } = new Vector();
		public float ExtraDamage { get; set; }
		public bool UseOriginator { get; set; }
		public bool SendAlert { get; set; }
		public AttackType AttackType { get; set; }
		public ulong HitType { get; set; }
		public DamageType DamageType { get; set; }
		public ulong CollisionFlag { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.ReactionTimeMin, endianess);
			output.WriteValueF32(this.ReactionTimeMax, endianess);
			output.WriteValueF32(this.ReactionDistanceMin, endianess);
			output.WriteValueF32(this.ReactionDistanceMax, endianess);
			output.WriteValueF32(this.ReactionHeightMin, endianess);
			output.WriteValueF32(this.ReactionHeightMax, endianess);
			this.ReactionDirection.Serialize(output, endianess);
			output.WriteValueF32(this.ExtraDamage, endianess);
			output.WriteValueB32(this.UseOriginator, endianess);
			output.WriteValueB32(this.SendAlert, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitType, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			output.WriteValueU64(this.CollisionFlag, endianess);
			this.Branch.Serialize(output, endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ReactionTimeMin = input.ReadValueF32(endianess);
			this.ReactionTimeMax = input.ReadValueF32(endianess);
			this.ReactionDistanceMin = input.ReadValueF32(endianess);
			this.ReactionDistanceMax = input.ReadValueF32(endianess);
			this.ReactionHeightMin = input.ReadValueF32(endianess);
			this.ReactionHeightMax = input.ReadValueF32(endianess);
			this.ReactionDirection = new Vector(input, endianess);
			this.ExtraDamage = input.ReadValueF32(endianess);
			this.UseOriginator = input.ReadValueB32(endianess);
			this.SendAlert = input.ReadValueB32(endianess);
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitType = input.ReadValueU64(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.CollisionFlag = input.ReadValueU64(endianess);
			this.Branch = new BranchReference(input, endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.Conditions);
		}
	}
}
