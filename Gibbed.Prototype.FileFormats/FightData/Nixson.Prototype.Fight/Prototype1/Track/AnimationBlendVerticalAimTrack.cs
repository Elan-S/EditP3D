using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(13873716285010177872UL)]
	public class AnimationBlendVerticalAimTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float AngleRate { get; set; }
		public float AngleDefault { get; set; }
		public float AngleOffset { get; set; }
		public float AngleOffsetMin { get; set; }
		public float AngleMin { get; set; }
		public float AngleMax { get; set; }
		public float OffsetInterpolationDist { get; set; }
		public AnimationBlendVerticalAimTrack.InterpolationDirectionType InterpolationDirection { get; set; }
		public float Speed { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public ulong AnimUp { get; set; }
		public float AnimUpAngle { get; set; }
		public float AnimUpSyncFrame { get; set; }
		public float AnimUpStartFrame { get; set; }
		public float AnimUpEndFrame { get; set; }
		public float AnimUpSpeed { get; set; }
		public ulong AnimNeutral { get; set; }
		public float AnimNeutralAngle { get; set; }
		public float AnimNeutralSyncFrame { get; set; }
		public float AnimNeutralStartFrame { get; set; }
		public float AnimNeutralEndFrame { get; set; }
		public float AnimNeutralSpeed { get; set; }
		public ulong AnimDown { get; set; }
		public float AnimDownAngle { get; set; }
		public float AnimDownSyncFrame { get; set; }
		public float AnimDownStartFrame { get; set; }
		public float AnimDownEndFrame { get; set; }
		public float AnimDownSpeed { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public AnimationBlendVerticalAimTrack.TargetingModeType TargetingMode { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.AngleRate, endianess);
			output.WriteValueF32(this.AngleDefault, endianess);
			output.WriteValueF32(this.AngleOffset, endianess);
			output.WriteValueF32(this.AngleOffsetMin, endianess);
			output.WriteValueF32(this.AngleMin, endianess);
			output.WriteValueF32(this.AngleMax, endianess);
			output.WriteValueF32(this.OffsetInterpolationDist, endianess);
			BaseProperty.SerializePropertyEnum<AnimationBlendVerticalAimTrack.InterpolationDirectionType>(output, endianess, this.InterpolationDirection);
			output.WriteValueF32(this.Speed, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueU64(this.AnimUp, endianess);
			output.WriteValueF32(this.AnimUpAngle, endianess);
			output.WriteValueF32(this.AnimUpSyncFrame, endianess);
			output.WriteValueF32(this.AnimUpStartFrame, endianess);
			output.WriteValueF32(this.AnimUpEndFrame, endianess);
			output.WriteValueF32(this.AnimUpSpeed, endianess);
			output.WriteValueU64(this.AnimNeutral, endianess);
			output.WriteValueF32(this.AnimNeutralAngle, endianess);
			output.WriteValueF32(this.AnimNeutralSyncFrame, endianess);
			output.WriteValueF32(this.AnimNeutralStartFrame, endianess);
			output.WriteValueF32(this.AnimNeutralEndFrame, endianess);
			output.WriteValueF32(this.AnimNeutralSpeed, endianess);
			output.WriteValueU64(this.AnimDown, endianess);
			output.WriteValueF32(this.AnimDownAngle, endianess);
			output.WriteValueF32(this.AnimDownSyncFrame, endianess);
			output.WriteValueF32(this.AnimDownStartFrame, endianess);
			output.WriteValueF32(this.AnimDownEndFrame, endianess);
			output.WriteValueF32(this.AnimDownSpeed, endianess);
			output.WriteValueU64(this.Partition, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			BaseProperty.SerializePropertyEnum<AnimationBlendVerticalAimTrack.TargetingModeType>(output, endianess, this.TargetingMode);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.AngleRate = input.ReadValueF32(endianess);
			this.AngleDefault = input.ReadValueF32(endianess);
			this.AngleOffset = input.ReadValueF32(endianess);
			this.AngleOffsetMin = input.ReadValueF32(endianess);
			this.AngleMin = input.ReadValueF32(endianess);
			this.AngleMax = input.ReadValueF32(endianess);
			this.OffsetInterpolationDist = input.ReadValueF32(endianess);
			this.InterpolationDirection = BaseProperty.DeserializePropertyEnum<AnimationBlendVerticalAimTrack.InterpolationDirectionType>(input, endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.AnimUp = input.ReadValueU64(endianess);
			this.AnimUpAngle = input.ReadValueF32(endianess);
			this.AnimUpSyncFrame = input.ReadValueF32(endianess);
			this.AnimUpStartFrame = input.ReadValueF32(endianess);
			this.AnimUpEndFrame = input.ReadValueF32(endianess);
			this.AnimUpSpeed = input.ReadValueF32(endianess);
			this.AnimNeutral = input.ReadValueU64(endianess);
			this.AnimNeutralAngle = input.ReadValueF32(endianess);
			this.AnimNeutralSyncFrame = input.ReadValueF32(endianess);
			this.AnimNeutralStartFrame = input.ReadValueF32(endianess);
			this.AnimNeutralEndFrame = input.ReadValueF32(endianess);
			this.AnimNeutralSpeed = input.ReadValueF32(endianess);
			this.AnimDown = input.ReadValueU64(endianess);
			this.AnimDownAngle = input.ReadValueF32(endianess);
			this.AnimDownSyncFrame = input.ReadValueF32(endianess);
			this.AnimDownStartFrame = input.ReadValueF32(endianess);
			this.AnimDownEndFrame = input.ReadValueF32(endianess);
			this.AnimDownSpeed = input.ReadValueF32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.TargetingMode = BaseProperty.DeserializePropertyEnum<AnimationBlendVerticalAimTrack.TargetingModeType>(input, endianess);
		}
		public enum InterpolationDirectionType : ulong
		{
			X = 88UL,
			Y,
			Z,
			XZ = 5772786UL,
			Magnitude = 2822888922154341390UL,
			AbsX = 18348266184121448UL,
			AbsY,
			AbsZ,
			WorldY = 6581076010052170401UL,
			WorldXZ = 2853626387689600314UL
		}
		public enum TargetingModeType : ulong
		{
			Target = 856854631462190855UL,
			TargetFeet = 697244243143271027UL,
			Intention = 9713923695382230984UL,
			Motion = 6297553359567207254UL
		}
	}
}
