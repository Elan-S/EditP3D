using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10208858318810067619UL)]
	public class JumpToTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool UseTarget { get; set; }
		public JumpToTrack.JumpToChargeType ChargeType { get; set; }
		public float DistanceMin { get; set; }
		public float DistanceMax { get; set; }
		public bool UseDistanceWhenUsingTarget { get; set; }
		public float MinInitialVelocityMin { get; set; }
		public float MinInitialVelocityMax { get; set; }
		public float MaxInitialVelocityMin { get; set; }
		public float MaxInitialVelocityMax { get; set; }
		public float TrackingMin { get; set; }
		public float TrackingMax { get; set; }
		public float MinSpeedTracking { get; set; }
		public float LaunchAngle { get; set; }
		public float MinLaunchAngle { get; set; }
		public float Gravity { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.UseTarget, endianess);
			BaseProperty.SerializePropertyEnum<JumpToTrack.JumpToChargeType>(output, endianess, this.ChargeType);
			output.WriteValueF32(this.DistanceMin, endianess);
			output.WriteValueF32(this.DistanceMax, endianess);
			output.WriteValueB32(this.UseDistanceWhenUsingTarget, endianess);
			output.WriteValueF32(this.MinInitialVelocityMin, endianess);
			output.WriteValueF32(this.MinInitialVelocityMax, endianess);
			output.WriteValueF32(this.MaxInitialVelocityMin, endianess);
			output.WriteValueF32(this.MaxInitialVelocityMax, endianess);
			output.WriteValueF32(this.TrackingMin, endianess);
			output.WriteValueF32(this.TrackingMax, endianess);
			output.WriteValueF32(this.MinSpeedTracking, endianess);
			output.WriteValueF32(this.LaunchAngle, endianess);
			output.WriteValueF32(this.MinLaunchAngle, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.UseTarget = input.ReadValueB32(endianess);
			this.ChargeType = BaseProperty.DeserializePropertyEnum<JumpToTrack.JumpToChargeType>(input, endianess);
			this.DistanceMin = input.ReadValueF32(endianess);
			this.DistanceMax = input.ReadValueF32(endianess);
			this.UseDistanceWhenUsingTarget = input.ReadValueB32(endianess);
			this.MinInitialVelocityMin = input.ReadValueF32(endianess);
			this.MinInitialVelocityMax = input.ReadValueF32(endianess);
			this.MaxInitialVelocityMin = input.ReadValueF32(endianess);
			this.MaxInitialVelocityMax = input.ReadValueF32(endianess);
			this.TrackingMin = input.ReadValueF32(endianess);
			this.TrackingMax = input.ReadValueF32(endianess);
			this.MinSpeedTracking = input.ReadValueF32(endianess);
			this.LaunchAngle = input.ReadValueF32(endianess);
			this.MinLaunchAngle = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.Offset = new Vector(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		public enum JumpToChargeType : ulong
		{
			Invalid = 4122290943349627157UL,
			Jump = 20889331387467136UL,
			Attack = 17648781240126830036UL,
			Throw = 5973634341500805012UL
		}
	}
}
