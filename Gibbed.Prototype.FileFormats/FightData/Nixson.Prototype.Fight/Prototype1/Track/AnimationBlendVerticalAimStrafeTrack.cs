using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.AnimationBlendVerticalAimStrafe)]
	public class AnimationBlendVerticalAimStrafeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong RefLocomotionName { get; set; }
		public float AngleRate { get; set; }
		public float AngleDefault { get; set; }
		public float AngleOffset { get; set; }
		public float AngleOffsetMin { get; set; }
		public float OffsetInterpolationDist { get; set; }
		public DirectionType InterpolationDirection { get; set; }
		public float Speed { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public ulong PivotJoint { get; set; }
		public Vector PivotOffset { get; set; } = new Vector();
		public Axis TargetingMode { get; set; }
		public float AnimUpAngle { get; set; }
		public float AnimNeutralAngle { get; set; }
		public float AnimDownAngle { get; set; }
		public ulong AnimIdleUp { get; set; }
		public ulong AnimIdleNeutral { get; set; }
		public ulong AnimIdleDown { get; set; }
		public ulong AnimNorthUp { get; set; }
		public ulong AnimNorthNeutral { get; set; }
		public ulong AnimNorthDown { get; set; }
		public ulong AnimEastUp { get; set; }
		public ulong AnimEastNeutral { get; set; }
		public ulong AnimEastDown { get; set; }
		public ulong AnimSouthUp { get; set; }
		public ulong AnimSouthNeutral { get; set; }
		public ulong AnimSouthDown { get; set; }
		public ulong AnimWestUp { get; set; }
		public ulong AnimWestNeutral { get; set; }
		public ulong AnimWestDown { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.RefLocomotionName, endianess);
			output.WriteValueF32(this.AngleRate, endianess);
			output.WriteValueF32(this.AngleDefault, endianess);
			output.WriteValueF32(this.AngleOffset, endianess);
			output.WriteValueF32(this.AngleOffsetMin, endianess);
			output.WriteValueF32(this.OffsetInterpolationDist, endianess);
			BaseProperty.SerializePropertyEnum<DirectionType>(output, endianess, this.InterpolationDirection);
			output.WriteValueF32(this.Speed, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueU64(this.PivotJoint, endianess);
			this.PivotOffset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<Axis>(output, endianess, this.TargetingMode);
			output.WriteValueF32(this.AnimUpAngle, endianess);
			output.WriteValueF32(this.AnimNeutralAngle, endianess);
			output.WriteValueF32(this.AnimDownAngle, endianess);
			output.WriteValueU64(this.AnimIdleUp, endianess);
			output.WriteValueU64(this.AnimIdleNeutral, endianess);
			output.WriteValueU64(this.AnimIdleDown, endianess);
			output.WriteValueU64(this.AnimNorthUp, endianess);
			output.WriteValueU64(this.AnimNorthNeutral, endianess);
			output.WriteValueU64(this.AnimNorthDown, endianess);
			output.WriteValueU64(this.AnimEastUp, endianess);
			output.WriteValueU64(this.AnimEastNeutral, endianess);
			output.WriteValueU64(this.AnimEastDown, endianess);
			output.WriteValueU64(this.AnimSouthUp, endianess);
			output.WriteValueU64(this.AnimSouthNeutral, endianess);
			output.WriteValueU64(this.AnimSouthDown, endianess);
			output.WriteValueU64(this.AnimWestUp, endianess);
			output.WriteValueU64(this.AnimWestNeutral, endianess);
			output.WriteValueU64(this.AnimWestDown, endianess);
			output.WriteValueU64(this.Partition, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.RefLocomotionName = input.ReadValueU64(endianess);
			this.AngleRate = input.ReadValueF32(endianess);
			this.AngleDefault = input.ReadValueF32(endianess);
			this.AngleOffset = input.ReadValueF32(endianess);
			this.AngleOffsetMin = input.ReadValueF32(endianess);
			this.OffsetInterpolationDist = input.ReadValueF32(endianess);
			this.InterpolationDirection = BaseProperty.DeserializePropertyEnum<DirectionType>(input, endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.PivotJoint = input.ReadValueU64(endianess);
			this.PivotOffset = new Vector(input, endianess);
			this.TargetingMode = BaseProperty.DeserializePropertyEnum<Axis>(input, endianess);
			this.AnimUpAngle = input.ReadValueF32(endianess);
			this.AnimNeutralAngle = input.ReadValueF32(endianess);
			this.AnimDownAngle = input.ReadValueF32(endianess);
			this.AnimIdleUp = input.ReadValueU64(endianess);
			this.AnimIdleNeutral = input.ReadValueU64(endianess);
			this.AnimIdleDown = input.ReadValueU64(endianess);
			this.AnimNorthUp = input.ReadValueU64(endianess);
			this.AnimNorthNeutral = input.ReadValueU64(endianess);
			this.AnimNorthDown = input.ReadValueU64(endianess);
			this.AnimEastUp = input.ReadValueU64(endianess);
			this.AnimEastNeutral = input.ReadValueU64(endianess);
			this.AnimEastDown = input.ReadValueU64(endianess);
			this.AnimSouthUp = input.ReadValueU64(endianess);
			this.AnimSouthNeutral = input.ReadValueU64(endianess);
			this.AnimSouthDown = input.ReadValueU64(endianess);
			this.AnimWestUp = input.ReadValueU64(endianess);
			this.AnimWestNeutral = input.ReadValueU64(endianess);
			this.AnimWestDown = input.ReadValueU64(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
