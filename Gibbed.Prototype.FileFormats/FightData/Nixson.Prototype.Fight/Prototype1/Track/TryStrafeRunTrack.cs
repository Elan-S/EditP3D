using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.TryStrafeRun)]
	public class TryStrafeRunTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinDistance { get; set; }
		public float MaxDistance { get; set; }
		public float MinAngle { get; set; }
		public float FreeRadiusEnd { get; set; }
		public float FreeRadiusTarget { get; set; }
		public int NumTries { get; set; }
		public float MinHeightAtTarget { get; set; }
		public float MaxHeightAtEnds { get; set; }
		public float MinHeightAtEnds { get; set; }
		public float SafeHeightAtEnds { get; set; }
		public float MaxHeightDiffTargetAndEnds { get; set; }
		public float MaxHeightDiffTargetAndWaypoint { get; set; }
		public float MaxPathHeightFromTarget { get; set; }
		public float MaxPathYaw { get; set; }
		public float MaxPathPitch { get; set; }
		public float FreeRadiusPath { get; set; }
		public int TurningFactorA { get; set; }
		public int TurningFactorB { get; set; }
		public TryStrafeRunTrack.HeuristicType Heuristic { get; set; }
		public bool SameHeightPath { get; set; }
		public bool Brake { get; set; }
		public float TimeBetweenTries { get; set; }
		public BranchReference IfFound { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MinDistance, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueF32(this.MinAngle, endianess);
			output.WriteValueF32(this.FreeRadiusEnd, endianess);
			output.WriteValueF32(this.FreeRadiusTarget, endianess);
			output.WriteValueS32(this.NumTries, endianess);
			output.WriteValueF32(this.MinHeightAtTarget, endianess);
			output.WriteValueF32(this.MaxHeightAtEnds, endianess);
			output.WriteValueF32(this.MinHeightAtEnds, endianess);
			output.WriteValueF32(this.SafeHeightAtEnds, endianess);
			output.WriteValueF32(this.MaxHeightDiffTargetAndEnds, endianess);
			output.WriteValueF32(this.MaxHeightDiffTargetAndWaypoint, endianess);
			output.WriteValueF32(this.MaxPathHeightFromTarget, endianess);
			output.WriteValueF32(this.MaxPathYaw, endianess);
			output.WriteValueF32(this.MaxPathPitch, endianess);
			output.WriteValueF32(this.FreeRadiusPath, endianess);
			output.WriteValueS32(this.TurningFactorA, endianess);
			output.WriteValueS32(this.TurningFactorB, endianess);
			BaseProperty.SerializePropertyEnum<TryStrafeRunTrack.HeuristicType>(output, endianess, this.Heuristic);
			output.WriteValueB32(this.SameHeightPath, endianess);
			output.WriteValueB32(this.Brake, endianess);
			output.WriteValueF32(this.TimeBetweenTries, endianess);
			this.IfFound.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MinDistance = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.MinAngle = input.ReadValueF32(endianess);
			this.FreeRadiusEnd = input.ReadValueF32(endianess);
			this.FreeRadiusTarget = input.ReadValueF32(endianess);
			this.NumTries = input.ReadValueS32(endianess);
			this.MinHeightAtTarget = input.ReadValueF32(endianess);
			this.MaxHeightAtEnds = input.ReadValueF32(endianess);
			this.MinHeightAtEnds = input.ReadValueF32(endianess);
			this.SafeHeightAtEnds = input.ReadValueF32(endianess);
			this.MaxHeightDiffTargetAndEnds = input.ReadValueF32(endianess);
			this.MaxHeightDiffTargetAndWaypoint = input.ReadValueF32(endianess);
			this.MaxPathHeightFromTarget = input.ReadValueF32(endianess);
			this.MaxPathYaw = input.ReadValueF32(endianess);
			this.MaxPathPitch = input.ReadValueF32(endianess);
			this.FreeRadiusPath = input.ReadValueF32(endianess);
			this.TurningFactorA = input.ReadValueS32(endianess);
			this.TurningFactorB = input.ReadValueS32(endianess);
			this.Heuristic = BaseProperty.DeserializePropertyEnum<TryStrafeRunTrack.HeuristicType>(input, endianess);
			this.SameHeightPath = input.ReadValueB32(endianess);
			this.Brake = input.ReadValueB32(endianess);
			this.TimeBetweenTries = input.ReadValueF32(endianess);
			this.IfFound.Deserialize(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.Conditions);
		}
		public enum HeuristicType : ulong
		{
			OnlyValid = 17132745256354547704UL,
			Lowest = 15981311668301248366UL
		}
	}
}
