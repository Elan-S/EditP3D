using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.SupportingLimbTest)]
	public class SupportingLimbTestTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong SupportingLimb { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public Vector Offset { get; set; } = new Vector();
		public float Radius { get; set; }
		public float Arc { get; set; }
		public float ExtraDistance { get; set; }
		public bool UseInput { get; set; }
		public float MaxInputArc { get; set; }
		public bool UsePreviousSurface { get; set; }
		public bool UseMotion { get; set; }
		public bool SwitchSurfaces { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public int Priority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.SupportingLimb, endianess);
			this.Direction.Serialize(output, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueF32(this.ExtraDistance, endianess);
			output.WriteValueB32(this.UseInput, endianess);
			output.WriteValueF32(this.MaxInputArc, endianess);
			output.WriteValueB32(this.UsePreviousSurface, endianess);
			output.WriteValueB32(this.UseMotion, endianess);
			output.WriteValueB32(this.SwitchSurfaces, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.SupportingLimb = input.ReadValueU64(endianess);
			this.Direction = new Vector(input, endianess);
			this.Offset = new Vector(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.ExtraDistance = input.ReadValueF32(endianess);
			this.UseInput = input.ReadValueB32(endianess);
			this.MaxInputArc = input.ReadValueF32(endianess);
			this.UsePreviousSurface = input.ReadValueB32(endianess);
			this.UseMotion = input.ReadValueB32(endianess);
			this.SwitchSurfaces = input.ReadValueB32(endianess);
			this.Branch = new BranchReference(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.Conditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.Conditions);
		}
	}
}
