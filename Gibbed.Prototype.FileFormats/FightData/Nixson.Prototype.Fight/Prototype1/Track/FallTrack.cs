using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Fall)]
	public class FallTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public float TurningVelocityMin { get; set; }
		public float TurningAccelerationMin { get; set; }
		public float TurningVelocityMinForwardSpeedCap { get; set; }
		public float TurningVelocityMax { get; set; }
		public float TurningAccelerationMax { get; set; }
		public float TurningVelocityMaxForwardSpeedCap { get; set; }
		public FallTrack.PreserveMomentumType PreserveMomentum { get; set; }
		public Vector InitialVelocity { get; set; } = new Vector();
		public float Gravity { get; set; }
		public float MaxVelocity { get; set; }
		public float GravityDecreaseRate { get; set; }
		public float MaxVelocityXY { get; set; }
		public float MaxVelocityUp { get; set; }
		public bool LimitTurning { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			output.WriteValueF32(this.TurningVelocityMin, endianess);
			output.WriteValueF32(this.TurningAccelerationMin, endianess);
			output.WriteValueF32(this.TurningVelocityMinForwardSpeedCap, endianess);
			output.WriteValueF32(this.TurningVelocityMax, endianess);
			output.WriteValueF32(this.TurningAccelerationMax, endianess);
			output.WriteValueF32(this.TurningVelocityMaxForwardSpeedCap, endianess);
			BaseProperty.SerializePropertyBitfield<FallTrack.PreserveMomentumType>(output, endianess, this.PreserveMomentum);
			this.InitialVelocity.Serialize(output, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueF32(this.MaxVelocity, endianess);
			output.WriteValueF32(this.GravityDecreaseRate, endianess);
			output.WriteValueF32(this.MaxVelocityXY, endianess);
			output.WriteValueF32(this.MaxVelocityUp, endianess);
			output.WriteValueB32(this.LimitTurning, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
			this.TurningVelocityMin = input.ReadValueF32(endianess);
			this.TurningAccelerationMin = input.ReadValueF32(endianess);
			this.TurningVelocityMinForwardSpeedCap = input.ReadValueF32(endianess);
			this.TurningVelocityMax = input.ReadValueF32(endianess);
			this.TurningAccelerationMax = input.ReadValueF32(endianess);
			this.TurningVelocityMaxForwardSpeedCap = input.ReadValueF32(endianess);
			this.PreserveMomentum = BaseProperty.DeserializePropertyBitfield<FallTrack.PreserveMomentumType>(input, endianess);
			this.InitialVelocity = new Vector(input, endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.MaxVelocity = input.ReadValueF32(endianess);
			this.GravityDecreaseRate = input.ReadValueF32(endianess);
			this.MaxVelocityXY = input.ReadValueF32(endianess);
			this.MaxVelocityUp = input.ReadValueF32(endianess);
			this.LimitTurning = input.ReadValueB32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		[Flags]
		public enum PreserveMomentumType : ulong
		{
			PreserveMomentum = 1UL,
			UseSprintVelocity = 2UL,
			SnapToFaceMotion = 4UL
		}
	}
}
