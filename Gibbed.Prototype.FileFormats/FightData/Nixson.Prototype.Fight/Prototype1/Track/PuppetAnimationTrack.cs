using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.PuppetAnimation)]
	public class PuppetAnimationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Animation { get; set; }
		public float Speed { get; set; }
		public float RandomSpeedVariation { get; set; }
		public float InitFrame { get; set; }
		public float StartFrame { get; set; }
		public float EndFrame { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public bool SyncFrame { get; set; }
		public bool PhaseMatch { get; set; }
		public bool ReuseExistingDriver { get; set; }
		public PuppetAnimationTrack.AnimationSyncPhase SyncPhase { get; set; }
		public float SyncPhaseMinFrame { get; set; }
		public float SyncPhaseMaxFrame { get; set; }
		public bool HasRootTranslation { get; set; }
		public bool HasRootRotation { get; set; }
		public ulong Partition { get; set; }
		public float Weight { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Animation, endianess);
			output.WriteValueF32(this.Speed, endianess);
			output.WriteValueF32(this.RandomSpeedVariation, endianess);
			output.WriteValueF32(this.InitFrame, endianess);
			output.WriteValueF32(this.StartFrame, endianess);
			output.WriteValueF32(this.EndFrame, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueB32(this.SyncFrame, endianess);
			output.WriteValueB32(this.PhaseMatch, endianess);
			output.WriteValueB32(this.ReuseExistingDriver, endianess);
			BaseProperty.SerializePropertyEnum<PuppetAnimationTrack.AnimationSyncPhase>(output, endianess, this.SyncPhase);
			output.WriteValueF32(this.SyncPhaseMinFrame, endianess);
			output.WriteValueF32(this.SyncPhaseMaxFrame, endianess);
			output.WriteValueB32(this.HasRootTranslation, endianess);
			output.WriteValueB32(this.HasRootRotation, endianess);
			output.WriteValueU64(this.Partition, endianess);
			output.WriteValueF32(this.Weight, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Animation = input.ReadValueU64(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.RandomSpeedVariation = input.ReadValueF32(endianess);
			this.InitFrame = input.ReadValueF32(endianess);
			this.StartFrame = input.ReadValueF32(endianess);
			this.EndFrame = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.SyncFrame = input.ReadValueB32(endianess);
			this.PhaseMatch = input.ReadValueB32(endianess);
			this.ReuseExistingDriver = input.ReadValueB32(endianess);
			this.SyncPhase = BaseProperty.DeserializePropertyEnum<PuppetAnimationTrack.AnimationSyncPhase>(input, endianess);
			this.SyncPhaseMinFrame = input.ReadValueF32(endianess);
			this.SyncPhaseMaxFrame = input.ReadValueF32(endianess);
			this.HasRootTranslation = input.ReadValueB32(endianess);
			this.HasRootRotation = input.ReadValueB32(endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Weight = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		public enum AnimationSyncPhase : ulong
		{
			Legacy = 3349652082457766725UL,
			FromPuppetPhase = 15638706115388059527UL
		}
	}
}
