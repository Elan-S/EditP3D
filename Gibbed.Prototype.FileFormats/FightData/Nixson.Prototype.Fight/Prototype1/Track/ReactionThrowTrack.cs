using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.ReactionThrow)]
	public class ReactionThrowTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float DamageMin { get; set; }
		public float DamageMax { get; set; }
		public Vector SpinAxisX { get; set; } = new Vector();
		public Vector SpinAxisY { get; set; } = new Vector();
		public float SelfDamageScale { get; set; }
		public bool UsePuppet { get; set; }
		public float OrientOffset { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public float PhysicalVelocityCheckDelay { get; set; }
		public float StopAtVelocityRatio { get; set; }
		public ulong ThreatName { get; set; }
		public float ThreatRadiusScale { get; set; }
		public float ThreatProjectedDist { get; set; }
		public BranchReference OnCollision { get; set; } = new BranchReference();
		public int CollisionInterruptPriority { get; set; }
		public float CollisionReactionDelay { get; set; }
		public int MaxHitEffects { get; set; }
		public TargetClass OverrideTargetClass { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.DamageMin, endianess);
			output.WriteValueF32(this.DamageMax, endianess);
			this.SpinAxisX.Serialize(output, endianess);
			this.SpinAxisY.Serialize(output, endianess);
			output.WriteValueF32(this.SelfDamageScale, endianess);
			output.WriteValueB32(this.UsePuppet, endianess);
			output.WriteValueF32(this.OrientOffset, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			output.WriteValueF32(this.PhysicalVelocityCheckDelay, endianess);
			output.WriteValueF32(this.StopAtVelocityRatio, endianess);
			output.WriteValueU64(this.ThreatName, endianess);
			output.WriteValueF32(this.ThreatRadiusScale, endianess);
			output.WriteValueF32(this.ThreatProjectedDist, endianess);
			this.OnCollision.Serialize(output, endianess);
			output.WriteValueS32(this.CollisionInterruptPriority, endianess);
			output.WriteValueF32(this.CollisionReactionDelay, endianess);
			output.WriteValueS32(this.MaxHitEffects, endianess);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.OverrideTargetClass);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.DamageMin = input.ReadValueF32(endianess);
			this.DamageMax = input.ReadValueF32(endianess);
			this.SpinAxisX = new Vector(input, endianess);
			this.SpinAxisY = new Vector(input, endianess);
			this.SelfDamageScale = input.ReadValueF32(endianess);
			this.UsePuppet = input.ReadValueB32(endianess);
			this.OrientOffset = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.PhysicalVelocityCheckDelay = input.ReadValueF32(endianess);
			this.StopAtVelocityRatio = input.ReadValueF32(endianess);
			this.ThreatName = input.ReadValueU64(endianess);
			this.ThreatRadiusScale = input.ReadValueF32(endianess);
			this.ThreatProjectedDist = input.ReadValueF32(endianess);
			this.OnCollision = new BranchReference(input, endianess);
			this.CollisionInterruptPriority = input.ReadValueS32(endianess);
			this.CollisionReactionDelay = input.ReadValueF32(endianess);
			this.MaxHitEffects = input.ReadValueS32(endianess);
			this.OverrideTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
		}
	}
}
