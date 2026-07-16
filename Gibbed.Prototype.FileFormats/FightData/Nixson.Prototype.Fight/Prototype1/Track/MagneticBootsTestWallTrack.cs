using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.MagneticBootsTestWall)]
	public class MagneticBootsTestWallTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public float YawOffset { get; set; }
		public float Length { get; set; }
		public ColliderType CollideWith { get; set; }
		public bool UseFacing { get; set; }
		public bool UseBackwardsFacing { get; set; }
		public bool DebugDraw { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueF32(this.YawOffset, endianess);
			output.WriteValueF32(this.Length, endianess);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.CollideWith);
			output.WriteValueB32(this.UseFacing, endianess);
			output.WriteValueB32(this.UseBackwardsFacing, endianess);
			output.WriteValueB32(this.DebugDraw, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Offset = new Vector(input, endianess);
			this.YawOffset = input.ReadValueF32(endianess);
			this.Length = input.ReadValueF32(endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.UseFacing = input.ReadValueB32(endianess);
			this.UseBackwardsFacing = input.ReadValueB32(endianess);
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
