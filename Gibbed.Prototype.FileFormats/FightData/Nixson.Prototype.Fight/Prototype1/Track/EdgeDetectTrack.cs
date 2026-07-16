using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.EdgeDetect)]
	public class EdgeDetectTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float SurfaceOffset { get; set; }
		public float LookaheadDistance { get; set; }
		public float EdgeDepth { get; set; }
		public Vector CardinalVector { get; set; } = new Vector();
		public float LookaheadDistanceNonCardinal { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.SurfaceOffset, endianess);
			output.WriteValueF32(this.LookaheadDistance, endianess);
			output.WriteValueF32(this.EdgeDepth, endianess);
			this.CardinalVector.Serialize(output, endianess);
			output.WriteValueF32(this.LookaheadDistanceNonCardinal, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.SurfaceOffset = input.ReadValueF32(endianess);
			this.LookaheadDistance = input.ReadValueF32(endianess);
			this.EdgeDepth = input.ReadValueF32(endianess);
			this.CardinalVector = new Vector(input, endianess);
			this.LookaheadDistanceNonCardinal = input.ReadValueF32(endianess);
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
