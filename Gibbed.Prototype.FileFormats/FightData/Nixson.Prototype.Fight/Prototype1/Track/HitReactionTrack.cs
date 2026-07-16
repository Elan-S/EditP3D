using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13353260699099125786UL)]
	public class HitReactionTrack : P1Track
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
		public AttackType AttackType { get; set; }
		public ulong HitType { get; set; }
		public DamageType DamageType { get; set; }
		public ulong CollisionFlag { get; set; }
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
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.AttackType);
			output.WriteValueU64(this.HitType, endianess);
			BaseProperty.SerializePropertyEnum<DamageType>(output, endianess, this.DamageType);
			output.WriteValueU64(this.CollisionFlag, endianess);
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
			this.AttackType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.HitType = input.ReadValueU64(endianess);
			this.DamageType = BaseProperty.DeserializePropertyEnum<DamageType>(input, endianess);
			this.CollisionFlag = input.ReadValueU64(endianess);
		}
	}
}
