using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.HitReactionThrow)]
	public class HitReactionThrowTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float AutoTargetArc { get; set; }
		public float MaxTargetArc { get; set; }
		public float DistanceMin { get; set; }
		public float DistanceMax { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float TrackingMin { get; set; }
		public float TrackingMax { get; set; }
		public float SpinMin { get; set; }
		public float SpinMax { get; set; }
		public float ArcRange { get; set; }
		public float ArcMax { get; set; }
		public float DamageMin { get; set; }
		public float DamageMax { get; set; }
		public float ImpulseMin { get; set; }
		public float ImpulseMax { get; set; }
		public float FriendlyFire { get; set; }
		public ulong HitType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.AutoTargetArc, endianess);
			output.WriteValueF32(this.MaxTargetArc, endianess);
			output.WriteValueF32(this.DistanceMin, endianess);
			output.WriteValueF32(this.DistanceMax, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.TrackingMin, endianess);
			output.WriteValueF32(this.TrackingMax, endianess);
			output.WriteValueF32(this.SpinMin, endianess);
			output.WriteValueF32(this.SpinMax, endianess);
			output.WriteValueF32(this.ArcRange, endianess);
			output.WriteValueF32(this.ArcMax, endianess);
			output.WriteValueF32(this.DamageMin, endianess);
			output.WriteValueF32(this.DamageMax, endianess);
			output.WriteValueF32(this.ImpulseMin, endianess);
			output.WriteValueF32(this.ImpulseMax, endianess);
			output.WriteValueF32(this.FriendlyFire, endianess);
			output.WriteValueU64(this.HitType, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.AutoTargetArc = input.ReadValueF32(endianess);
			this.MaxTargetArc = input.ReadValueF32(endianess);
			this.DistanceMin = input.ReadValueF32(endianess);
			this.DistanceMax = input.ReadValueF32(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.TrackingMin = input.ReadValueF32(endianess);
			this.TrackingMax = input.ReadValueF32(endianess);
			this.SpinMin = input.ReadValueF32(endianess);
			this.SpinMax = input.ReadValueF32(endianess);
			this.ArcRange = input.ReadValueF32(endianess);
			this.ArcMax = input.ReadValueF32(endianess);
			this.DamageMin = input.ReadValueF32(endianess);
			this.DamageMax = input.ReadValueF32(endianess);
			this.ImpulseMin = input.ReadValueF32(endianess);
			this.ImpulseMax = input.ReadValueF32(endianess);
			this.FriendlyFire = input.ReadValueF32(endianess);
			this.HitType = input.ReadValueU64(endianess);
		}
	}
}
