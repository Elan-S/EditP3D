using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12328431758273282105UL)]
	public class AnimationBlendDirectionalTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public AnimationBlendDirectionalTrack.InputAxisType Input { get; set; }
		public float BlendRate { get; set; }
		public float Speed { get; set; }
		public AnimationCyclic Cyclic { get; set; }
		public ulong AnimNeutral { get; set; }
		public float AnimNeutralSyncFrame { get; set; }
		public float AnimNeutralStartFrame { get; set; }
		public float AnimNeutralEndFrame { get; set; }
		public float AnimNeutralSpeed { get; set; }
		public ulong AnimNorth { get; set; }
		public float AnimNorthSyncFrame { get; set; }
		public float AnimNorthStartFrame { get; set; }
		public float AnimNorthEndFrame { get; set; }
		public float AnimNorthSpeed { get; set; }
		public AnimationAllowedTransitionsType AnimNorthAllowedTransitions { get; set; }
		public ulong AnimEast { get; set; }
		public float AnimEastSyncFrame { get; set; }
		public float AnimEastStartFrame { get; set; }
		public float AnimEastEndFrame { get; set; }
		public float AnimEastSpeed { get; set; }
		public AnimationAllowedTransitionsType AnimEastAllowedTransitions { get; set; }
		public ulong AnimSouth { get; set; }
		public float AnimSouthSyncFrame { get; set; }
		public float AnimSouthStartFrame { get; set; }
		public float AnimSouthEndFrame { get; set; }
		public float AnimSouthSpeed { get; set; }
		public AnimationAllowedTransitionsType AnimSouthAllowedTransitions { get; set; }
		public ulong AnimWest { get; set; }
		public float AnimWestSyncFrame { get; set; }
		public float AnimWestStartFrame { get; set; }
		public float AnimWestEndFrame { get; set; }
		public float AnimWestSpeed { get; set; }
		public AnimationAllowedTransitionsType AnimWestAllowedTransitions { get; set; }
		public ulong Partition { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<AnimationBlendDirectionalTrack.InputAxisType>(output, endianess, this.Input);
			output.WriteValueF32(this.BlendRate, endianess);
			output.WriteValueF32(this.Speed, endianess);
			BaseProperty.SerializePropertyEnum<AnimationCyclic>(output, endianess, this.Cyclic);
			output.WriteValueU64(this.AnimNeutral, endianess);
			output.WriteValueF32(this.AnimNeutralSyncFrame, endianess);
			output.WriteValueF32(this.AnimNeutralStartFrame, endianess);
			output.WriteValueF32(this.AnimNeutralEndFrame, endianess);
			output.WriteValueF32(this.AnimNeutralSpeed, endianess);
			output.WriteValueU64(this.AnimNorth, endianess);
			output.WriteValueF32(this.AnimNorthSyncFrame, endianess);
			output.WriteValueF32(this.AnimNorthStartFrame, endianess);
			output.WriteValueF32(this.AnimNorthEndFrame, endianess);
			output.WriteValueF32(this.AnimNorthSpeed, endianess);
			BaseProperty.SerializePropertyBitfield<AnimationAllowedTransitionsType>(output, endianess, this.AnimNorthAllowedTransitions);
			output.WriteValueU64(this.AnimEast, endianess);
			output.WriteValueF32(this.AnimEastSyncFrame, endianess);
			output.WriteValueF32(this.AnimEastStartFrame, endianess);
			output.WriteValueF32(this.AnimEastEndFrame, endianess);
			output.WriteValueF32(this.AnimEastSpeed, endianess);
			BaseProperty.SerializePropertyBitfield<AnimationAllowedTransitionsType>(output, endianess, this.AnimEastAllowedTransitions);
			output.WriteValueU64(this.AnimSouth, endianess);
			output.WriteValueF32(this.AnimSouthSyncFrame, endianess);
			output.WriteValueF32(this.AnimSouthStartFrame, endianess);
			output.WriteValueF32(this.AnimSouthEndFrame, endianess);
			output.WriteValueF32(this.AnimSouthSpeed, endianess);
			BaseProperty.SerializePropertyBitfield<AnimationAllowedTransitionsType>(output, endianess, this.AnimSouthAllowedTransitions);
			output.WriteValueU64(this.AnimWest, endianess);
			output.WriteValueF32(this.AnimWestSyncFrame, endianess);
			output.WriteValueF32(this.AnimWestStartFrame, endianess);
			output.WriteValueF32(this.AnimWestEndFrame, endianess);
			output.WriteValueF32(this.AnimWestSpeed, endianess);
			BaseProperty.SerializePropertyBitfield<AnimationAllowedTransitionsType>(output, endianess, this.AnimWestAllowedTransitions);
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
			this.Input = BaseProperty.DeserializePropertyEnum<AnimationBlendDirectionalTrack.InputAxisType>(input, endianess);
			this.BlendRate = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Cyclic = BaseProperty.DeserializePropertyEnum<AnimationCyclic>(input, endianess);
			this.AnimNeutral = input.ReadValueU64(endianess);
			this.AnimNeutralSyncFrame = input.ReadValueF32(endianess);
			this.AnimNeutralStartFrame = input.ReadValueF32(endianess);
			this.AnimNeutralEndFrame = input.ReadValueF32(endianess);
			this.AnimNeutralSpeed = input.ReadValueF32(endianess);
			this.AnimNorth = input.ReadValueU64(endianess);
			this.AnimNorthSyncFrame = input.ReadValueF32(endianess);
			this.AnimNorthStartFrame = input.ReadValueF32(endianess);
			this.AnimNorthEndFrame = input.ReadValueF32(endianess);
			this.AnimNorthSpeed = input.ReadValueF32(endianess);
			this.AnimNorthAllowedTransitions = BaseProperty.DeserializePropertyBitfield<AnimationAllowedTransitionsType>(input, endianess);
			this.AnimEast = input.ReadValueU64(endianess);
			this.AnimEastSyncFrame = input.ReadValueF32(endianess);
			this.AnimEastStartFrame = input.ReadValueF32(endianess);
			this.AnimEastEndFrame = input.ReadValueF32(endianess);
			this.AnimEastSpeed = input.ReadValueF32(endianess);
			this.AnimEastAllowedTransitions = BaseProperty.DeserializePropertyBitfield<AnimationAllowedTransitionsType>(input, endianess);
			this.AnimSouth = input.ReadValueU64(endianess);
			this.AnimSouthSyncFrame = input.ReadValueF32(endianess);
			this.AnimSouthStartFrame = input.ReadValueF32(endianess);
			this.AnimSouthEndFrame = input.ReadValueF32(endianess);
			this.AnimSouthSpeed = input.ReadValueF32(endianess);
			this.AnimSouthAllowedTransitions = BaseProperty.DeserializePropertyBitfield<AnimationAllowedTransitionsType>(input, endianess);
			this.AnimWest = input.ReadValueU64(endianess);
			this.AnimWestSyncFrame = input.ReadValueF32(endianess);
			this.AnimWestStartFrame = input.ReadValueF32(endianess);
			this.AnimWestEndFrame = input.ReadValueF32(endianess);
			this.AnimWestSpeed = input.ReadValueF32(endianess);
			this.AnimWestAllowedTransitions = BaseProperty.DeserializePropertyBitfield<AnimationAllowedTransitionsType>(input, endianess);
			this.Partition = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		public enum InputAxisType : ulong
		{
			MovementAxis = 1365744333596055274UL
		}
	}
}
