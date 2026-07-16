using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.ObstacleTest)]
	public class ObstacleTestTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float LookaheadDistance { get; set; }
		public float CharacterHeight { get; set; }
		public float CharacterWidth { get; set; }
		public float CharacterDepth { get; set; }
		public float CharacterSurfaceClearance { get; set; }
		public float ObstacleHeightMin { get; set; }
		public float ObstacleHeightMax { get; set; }
		public float ObstacleLengthMax { get; set; }
		public Vector SurfaceNormalOverride { get; set; } = new Vector();
		public float PoleAvoidVelocity { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.LookaheadDistance, endianess);
			output.WriteValueF32(this.CharacterHeight, endianess);
			output.WriteValueF32(this.CharacterWidth, endianess);
			output.WriteValueF32(this.CharacterDepth, endianess);
			output.WriteValueF32(this.CharacterSurfaceClearance, endianess);
			output.WriteValueF32(this.ObstacleHeightMin, endianess);
			output.WriteValueF32(this.ObstacleHeightMax, endianess);
			output.WriteValueF32(this.ObstacleLengthMax, endianess);
			this.SurfaceNormalOverride.Serialize(output, endianess);
			output.WriteValueF32(this.PoleAvoidVelocity, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.LookaheadDistance = input.ReadValueF32(endianess);
			this.CharacterHeight = input.ReadValueF32(endianess);
			this.CharacterWidth = input.ReadValueF32(endianess);
			this.CharacterDepth = input.ReadValueF32(endianess);
			this.CharacterSurfaceClearance = input.ReadValueF32(endianess);
			this.ObstacleHeightMin = input.ReadValueF32(endianess);
			this.ObstacleHeightMax = input.ReadValueF32(endianess);
			this.ObstacleLengthMax = input.ReadValueF32(endianess);
			this.SurfaceNormalOverride = new Vector(input, endianess);
			this.PoleAvoidVelocity = input.ReadValueF32(endianess);
			this.Branch = new BranchReference(input, endianess);
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
	}
}
