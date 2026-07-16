using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.MagneticBootsTestCorner)]
	public class MagneticBootsTestCornerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float LookAhead { get; set; }
		public float CornerDepth { get; set; }
		public float CornerLookBack { get; set; }
		public ColliderType CollideWith { get; set; }
		public bool DebugDraw { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.LookAhead, endianess);
			output.WriteValueF32(this.CornerDepth, endianess);
			output.WriteValueF32(this.CornerLookBack, endianess);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWith);
			output.WriteValueB32(this.DebugDraw, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.LookAhead = input.ReadValueF32(endianess);
			this.CornerDepth = input.ReadValueF32(endianess);
			this.CornerLookBack = input.ReadValueF32(endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.DebugDraw = input.ReadValueB32(endianess);
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
