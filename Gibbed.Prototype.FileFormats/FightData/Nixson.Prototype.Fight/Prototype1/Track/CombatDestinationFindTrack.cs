using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.AI)]
	[KnownTrack(TrackHash.CombatDestinationFind)]
	public class CombatDestinationFindTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinDistance { get; set; }
		public float MaxDistance { get; set; }
		public float MinAngle { get; set; }
		public float MaxAngle { get; set; }
		public int NumTries { get; set; }
		public bool IgnoreRestrictions { get; set; }
		public bool CheckNotReserved { get; set; }
		public bool CheckCanGo { get; set; }
		public bool UseNavMeshHeight { get; set; }
		public CombatDestinationFindTrack.HeightDiferenceType HeightDifferenceCheck { get; set; }
		public float MaxHeightDifference { get; set; }
		public float MinHeightDifference { get; set; }
		public float LookJustCloser { get; set; }
		public float MinDistanceFromCurrent { get; set; }
		public float FreeRadiusAround { get; set; }
		public AllowedAreasFlags AllowedAreas { get; set; }
		public bool IgnoreAreasWhenAgentOutside { get; set; }
		public float MaxDistanceToGround { get; set; }
		public float MaxAbsoluteHeight { get; set; }
		public Vector ReferenceOffset { get; set; } = new Vector();
		public bool AirDestination { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public CombatDestinationFindTrack.CameraVisibilityType CameraVisibility { get; set; }
		public CollisionFlagsType CollisionFlagsLOF { get; set; }
		public ColliderType CollideWithTypeLOF { get; set; }
		public Vector OffsetLOF { get; set; } = new Vector();
		public HeuristicType Heuristic { get; set; }
		public int InitialHeuristic { get; set; }
		public ulong InitialHeuristicTimer { get; set; }
		public float InitialHeuristicMinTimer { get; set; }
		public float InitialHeuristicMinTimerFactor { get; set; }
		public float InitialHeuristicMaxTimer { get; set; }
		public float InitialHeuristicMaxTimerFactor { get; set; }
		public CombatDestinationFindTrack.ReferencePositionType ReferencePosition { get; set; }
		public float DeadReckoningTime { get; set; }
		public BranchReference WhenFound { get; set; } = new BranchReference();
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
			output.WriteValueF32(this.MaxAngle, endianess);
			output.WriteValueS32(this.NumTries, endianess);
			output.WriteValueB32(this.IgnoreRestrictions, endianess);
			output.WriteValueB32(this.CheckNotReserved, endianess);
			output.WriteValueB32(this.CheckCanGo, endianess);
			output.WriteValueB32(this.UseNavMeshHeight, endianess);
			BaseProperty.SerializePropertyEnum<CombatDestinationFindTrack.HeightDiferenceType>(output, endianess, this.HeightDifferenceCheck);
			output.WriteValueF32(this.MaxHeightDifference, endianess);
			output.WriteValueF32(this.MinHeightDifference, endianess);
			output.WriteValueF32(this.LookJustCloser, endianess);
			output.WriteValueF32(this.MinDistanceFromCurrent, endianess);
			output.WriteValueF32(this.FreeRadiusAround, endianess);
			BaseProperty.SerializePropertyBitfield<AllowedAreasFlags>(output, endianess, this.AllowedAreas);
			output.WriteValueB32(this.IgnoreAreasWhenAgentOutside, endianess);
			output.WriteValueF32(this.MaxDistanceToGround, endianess);
			output.WriteValueF32(this.MaxAbsoluteHeight, endianess);
			this.ReferenceOffset.Serialize(output, endianess);
			output.WriteValueB32(this.AirDestination, endianess);
			this.Offset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CombatDestinationFindTrack.CameraVisibilityType>(output, endianess, this.CameraVisibility);
			BaseProperty.SerializePropertyEnum<CollisionFlagsType>(output, endianess, this.CollisionFlagsLOF);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWithTypeLOF);
			this.OffsetLOF.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<HeuristicType>(output, endianess, this.Heuristic);
			output.WriteValueS32(this.InitialHeuristic, endianess);
			output.WriteValueU64(this.InitialHeuristicTimer, endianess);
			output.WriteValueF32(this.InitialHeuristicMinTimer, endianess);
			output.WriteValueF32(this.InitialHeuristicMinTimerFactor, endianess);
			output.WriteValueF32(this.InitialHeuristicMaxTimer, endianess);
			output.WriteValueF32(this.InitialHeuristicMaxTimerFactor, endianess);
			BaseProperty.SerializePropertyEnum<CombatDestinationFindTrack.ReferencePositionType>(output, endianess, this.ReferencePosition);
			output.WriteValueF32(this.DeadReckoningTime, endianess);
			this.WhenFound.Serialize(output, endianess);
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
			this.MaxAngle = input.ReadValueF32(endianess);
			this.NumTries = input.ReadValueS32(endianess);
			this.IgnoreRestrictions = input.ReadValueB32(endianess);
			this.CheckNotReserved = input.ReadValueB32(endianess);
			this.CheckCanGo = input.ReadValueB32(endianess);
			this.UseNavMeshHeight = input.ReadValueB32(endianess);
			this.HeightDifferenceCheck = BaseProperty.DeserializePropertyEnum<CombatDestinationFindTrack.HeightDiferenceType>(input, endianess);
			this.MaxHeightDifference = input.ReadValueF32(endianess);
			this.MinHeightDifference = input.ReadValueF32(endianess);
			this.LookJustCloser = input.ReadValueF32(endianess);
			this.MinDistanceFromCurrent = input.ReadValueF32(endianess);
			this.FreeRadiusAround = input.ReadValueF32(endianess);
			this.AllowedAreas = BaseProperty.DeserializePropertyBitfield<AllowedAreasFlags>(input, endianess);
			this.IgnoreAreasWhenAgentOutside = input.ReadValueB32(endianess);
			this.MaxDistanceToGround = input.ReadValueF32(endianess);
			this.MaxAbsoluteHeight = input.ReadValueF32(endianess);
			this.ReferenceOffset = new Vector(input, endianess);
			this.AirDestination = input.ReadValueB32(endianess);
			this.Offset = new Vector(input, endianess);
			this.CameraVisibility = BaseProperty.DeserializePropertyEnum<CombatDestinationFindTrack.CameraVisibilityType>(input, endianess);
			this.CollisionFlagsLOF = BaseProperty.DeserializePropertyEnum<CollisionFlagsType>(input, endianess);
			this.CollideWithTypeLOF = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.OffsetLOF = new Vector(input, endianess);
			this.Heuristic = BaseProperty.DeserializePropertyEnum<HeuristicType>(input, endianess);
			this.InitialHeuristic = input.ReadValueS32(endianess);
			this.InitialHeuristicTimer = input.ReadValueU64(endianess);
			this.InitialHeuristicMinTimer = input.ReadValueF32(endianess);
			this.InitialHeuristicMinTimerFactor = input.ReadValueF32(endianess);
			this.InitialHeuristicMaxTimer = input.ReadValueF32(endianess);
			this.InitialHeuristicMaxTimerFactor = input.ReadValueF32(endianess);
			this.ReferencePosition = BaseProperty.DeserializePropertyEnum<CombatDestinationFindTrack.ReferencePositionType>(input, endianess);
			this.DeadReckoningTime = input.ReadValueF32(endianess);
			this.WhenFound = new BranchReference(input, endianess);
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
		public enum HeightDiferenceType : ulong
		{
			Absolute = 11751081574227249751UL,
			GreaterThan = 6405607611880470083UL,
			LessThan = 18107689237601833186UL
		}
		public enum CameraVisibilityType : ulong
		{
			None = 31052086116886518UL,
			Outside = 15663523954389067717UL,
			Inside = 9915946547602718564UL
		}
		public enum ReferencePositionType : ulong
		{
			Target = 12772528083933579047UL,
			Me = 7150262UL,
			FixedArea = 15026010715474639685UL,
			Anchor = 11548714336321995175UL
		}
	}
}
