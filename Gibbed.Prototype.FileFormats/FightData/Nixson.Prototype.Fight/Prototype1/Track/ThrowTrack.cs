using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Throw)]
	public class ThrowTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong GrabSlot { get; set; }
		public float DirectionX { get; set; }
		public float DirectionY { get; set; }
		public float DirectionZ { get; set; }
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
		public bool UseTarget { get; set; }
		public bool UseTargetVehicle { get; set; }
		public float RandomOffset { get; set; }
		public int InterruptPriority { get; set; }
		public ulong HitType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			output.WriteValueF32(this.DirectionX, endianess);
			output.WriteValueF32(this.DirectionY, endianess);
			output.WriteValueF32(this.DirectionZ, endianess);
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
			output.WriteValueB32(this.UseTarget, endianess);
			output.WriteValueB32(this.UseTargetVehicle, endianess);
			output.WriteValueF32(this.RandomOffset, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
			output.WriteValueU64(this.HitType, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.DirectionX = input.ReadValueF32(endianess);
			this.DirectionY = input.ReadValueF32(endianess);
			this.DirectionZ = input.ReadValueF32(endianess);
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
			this.UseTarget = input.ReadValueB32(endianess);
			this.UseTargetVehicle = input.ReadValueB32(endianess);
			this.RandomOffset = input.ReadValueF32(endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
			this.HitType = input.ReadValueU64(endianess);
		}
	}
}
