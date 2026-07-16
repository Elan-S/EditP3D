using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.LocoSprint)]
	public class LocoSprintTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMid { get; set; }
		public float VelocityMax { get; set; }
		public float AccelerationMin { get; set; }
		public float AccelerationMid { get; set; }
		public float AccelerationMax { get; set; }
		public float Deceleration { get; set; }
		public float TurnVelocityMin { get; set; }
		public float TurnVelocityMax { get; set; }
		public float TurnAccelerationMin { get; set; }
		public float TurnAccelerationMax { get; set; }
		public float TurnDeceleration { get; set; }
		public float LeanRate { get; set; }
		public float Phase { get; set; }
		public ulong Locomotion { get; set; }
		public ulong AnimMin { get; set; }
		public ulong AnimMinLeanEast { get; set; }
		public ulong AnimMinLeanWest { get; set; }
		public ulong AnimMid { get; set; }
		public ulong AnimMidLeanEast { get; set; }
		public ulong AnimMidLeanWest { get; set; }
		public ulong AnimMax { get; set; }
		public ulong AnimMaxLeanEast { get; set; }
		public ulong AnimMaxLeanWest { get; set; }
		public bool ForceAnimationVelocities { get; set; }
		public float UnlockableVelocityMin { get; set; }
		public UnlockableEnum UnlockableFirst { get; set; }
		public UnlockableEnum UnlockableLast { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMid, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.AccelerationMin, endianess);
			output.WriteValueF32(this.AccelerationMid, endianess);
			output.WriteValueF32(this.AccelerationMax, endianess);
			output.WriteValueF32(this.Deceleration, endianess);
			output.WriteValueF32(this.TurnVelocityMin, endianess);
			output.WriteValueF32(this.TurnVelocityMax, endianess);
			output.WriteValueF32(this.TurnAccelerationMin, endianess);
			output.WriteValueF32(this.TurnAccelerationMax, endianess);
			output.WriteValueF32(this.TurnDeceleration, endianess);
			output.WriteValueF32(this.LeanRate, endianess);
			output.WriteValueF32(this.Phase, endianess);
			output.WriteValueU64(this.Locomotion, endianess);
			output.WriteValueU64(this.AnimMin, endianess);
			output.WriteValueU64(this.AnimMinLeanEast, endianess);
			output.WriteValueU64(this.AnimMinLeanWest, endianess);
			output.WriteValueU64(this.AnimMid, endianess);
			output.WriteValueU64(this.AnimMidLeanEast, endianess);
			output.WriteValueU64(this.AnimMidLeanWest, endianess);
			output.WriteValueU64(this.AnimMax, endianess);
			output.WriteValueU64(this.AnimMaxLeanEast, endianess);
			output.WriteValueU64(this.AnimMaxLeanWest, endianess);
			output.WriteValueB32(this.ForceAnimationVelocities, endianess);
			output.WriteValueF32(this.UnlockableVelocityMin, endianess);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableFirst);
			BaseProperty.SerializePropertyEnum<UnlockableEnum>(output, endianess, this.UnlockableLast);
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
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMid = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.AccelerationMin = input.ReadValueF32(endianess);
			this.AccelerationMid = input.ReadValueF32(endianess);
			this.AccelerationMax = input.ReadValueF32(endianess);
			this.Deceleration = input.ReadValueF32(endianess);
			this.TurnVelocityMin = input.ReadValueF32(endianess);
			this.TurnVelocityMax = input.ReadValueF32(endianess);
			this.TurnAccelerationMin = input.ReadValueF32(endianess);
			this.TurnAccelerationMax = input.ReadValueF32(endianess);
			this.TurnDeceleration = input.ReadValueF32(endianess);
			this.LeanRate = input.ReadValueF32(endianess);
			this.Phase = input.ReadValueF32(endianess);
			this.Locomotion = input.ReadValueU64(endianess);
			this.AnimMin = input.ReadValueU64(endianess);
			this.AnimMinLeanEast = input.ReadValueU64(endianess);
			this.AnimMinLeanWest = input.ReadValueU64(endianess);
			this.AnimMid = input.ReadValueU64(endianess);
			this.AnimMidLeanEast = input.ReadValueU64(endianess);
			this.AnimMidLeanWest = input.ReadValueU64(endianess);
			this.AnimMax = input.ReadValueU64(endianess);
			this.AnimMaxLeanEast = input.ReadValueU64(endianess);
			this.AnimMaxLeanWest = input.ReadValueU64(endianess);
			this.ForceAnimationVelocities = input.ReadValueB32(endianess);
			this.UnlockableVelocityMin = input.ReadValueF32(endianess);
			this.UnlockableFirst = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
			this.UnlockableLast = BaseProperty.DeserializePropertyEnum<UnlockableEnum>(input, endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
