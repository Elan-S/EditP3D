using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10029552170906483387UL)]
	public class WallJumpUpTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float FlightTimeMin { get; set; }
		public float FlightTimeMax { get; set; }
		public float HeightMin { get; set; }
		public float HeightMax { get; set; }
		public float TurnVelocityMin { get; set; }
		public float TurnVelocityMax { get; set; }
		public float TurnAccelerationMin { get; set; }
		public float TurnAccelerationMax { get; set; }
		public UnlockableEnum UnlockableFirst { get; set; }
		public UnlockableEnum UnlockableLast { get; set; }
		public float UnlockableFlightTimeMin { get; set; }
		public float UnlockableHeightMin { get; set; }
		public float ObstacleLookaheadDistance { get; set; }
		public float CharacterHeight { get; set; }
		public float CharacterWidth { get; set; }
		public float CharacterDepth { get; set; }
		public float CharacterSurfaceClearance { get; set; }
		public float ObstacleHeightMin { get; set; }
		public float ObstacleHeightMax { get; set; }
		public float ObstacleLengthMax { get; set; }
		public float PushOutHeightMin { get; set; }
		public float PushOutVelocityMax { get; set; }
		public float ObstacleFaceHeightMin { get; set; }
		public float PushOutHeightMax { get; set; }
		public float ObstacleSlopeMax { get; set; }
		public float AbortRayLength { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.FlightTimeMin, endianess);
			output.WriteValueF32(this.FlightTimeMax, endianess);
			output.WriteValueF32(this.HeightMin, endianess);
			output.WriteValueF32(this.HeightMax, endianess);
			output.WriteValueF32(this.TurnVelocityMin, endianess);
			output.WriteValueF32(this.TurnVelocityMax, endianess);
			output.WriteValueF32(this.TurnAccelerationMin, endianess);
			output.WriteValueF32(this.TurnAccelerationMax, endianess);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableFirst);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableLast);
			output.WriteValueF32(this.UnlockableFlightTimeMin, endianess);
			output.WriteValueF32(this.UnlockableHeightMin, endianess);
			output.WriteValueF32(this.ObstacleLookaheadDistance, endianess);
			output.WriteValueF32(this.CharacterHeight, endianess);
			output.WriteValueF32(this.CharacterWidth, endianess);
			output.WriteValueF32(this.CharacterDepth, endianess);
			output.WriteValueF32(this.CharacterSurfaceClearance, endianess);
			output.WriteValueF32(this.ObstacleHeightMin, endianess);
			output.WriteValueF32(this.ObstacleHeightMax, endianess);
			output.WriteValueF32(this.ObstacleLengthMax, endianess);
			output.WriteValueF32(this.PushOutHeightMin, endianess);
			output.WriteValueF32(this.PushOutVelocityMax, endianess);
			output.WriteValueF32(this.ObstacleFaceHeightMin, endianess);
			output.WriteValueF32(this.PushOutHeightMax, endianess);
			output.WriteValueF32(this.ObstacleSlopeMax, endianess);
			output.WriteValueF32(this.AbortRayLength, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.FlightTimeMin = input.ReadValueF32(endianess);
			this.FlightTimeMax = input.ReadValueF32(endianess);
			this.HeightMin = input.ReadValueF32(endianess);
			this.HeightMax = input.ReadValueF32(endianess);
			this.TurnVelocityMin = input.ReadValueF32(endianess);
			this.TurnVelocityMax = input.ReadValueF32(endianess);
			this.TurnAccelerationMin = input.ReadValueF32(endianess);
			this.TurnAccelerationMax = input.ReadValueF32(endianess);
			this.UnlockableFirst = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
			this.UnlockableLast = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
			this.UnlockableFlightTimeMin = input.ReadValueF32(endianess);
			this.UnlockableHeightMin = input.ReadValueF32(endianess);
			this.ObstacleLookaheadDistance = input.ReadValueF32(endianess);
			this.CharacterHeight = input.ReadValueF32(endianess);
			this.CharacterWidth = input.ReadValueF32(endianess);
			this.CharacterDepth = input.ReadValueF32(endianess);
			this.CharacterSurfaceClearance = input.ReadValueF32(endianess);
			this.ObstacleHeightMin = input.ReadValueF32(endianess);
			this.ObstacleHeightMax = input.ReadValueF32(endianess);
			this.ObstacleLengthMax = input.ReadValueF32(endianess);
			this.PushOutHeightMin = input.ReadValueF32(endianess);
			this.PushOutVelocityMax = input.ReadValueF32(endianess);
			this.ObstacleFaceHeightMin = input.ReadValueF32(endianess);
			this.PushOutHeightMax = input.ReadValueF32(endianess);
			this.ObstacleSlopeMax = input.ReadValueF32(endianess);
			this.AbortRayLength = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
