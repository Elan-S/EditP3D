using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10019673881396366907UL)]
	public class FlyingAutoTargetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Velocity { get; set; }
		public float TurningVelocity { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public float Arc { get; set; }
		public float MaxDistance { get; set; }
		public bool CheckArc { get; set; }
		public bool CheckDistance { get; set; }
		public TargetFilterType TargetFilter { get; set; }
		public bool StickToTarget { get; set; }
		public bool UseExistingAutoTarget { get; set; }
		public bool UseReactionHitEvent { get; set; }
		public float CharacterHeight { get; set; }
		public float CharacterWidth { get; set; }
		public TargetClass MinPriorityTargetClass { get; set; }
		public TargetClass MaxPriorityTargetClass { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Velocity, endianess);
			output.WriteValueF32(this.TurningVelocity, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueB32(this.CheckArc, endianess);
			output.WriteValueB32(this.CheckDistance, endianess);
			BaseProperty.SerializePropertyBitfield<TargetFilterType>(output, endianess, this.TargetFilter);
			output.WriteValueB32(this.StickToTarget, endianess);
			output.WriteValueB32(this.UseExistingAutoTarget, endianess);
			output.WriteValueB32(this.UseReactionHitEvent, endianess);
			output.WriteValueF32(this.CharacterHeight, endianess);
			output.WriteValueF32(this.CharacterWidth, endianess);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.MinPriorityTargetClass);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.MaxPriorityTargetClass);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Velocity = input.ReadValueF32(endianess);
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.Offset = new Vector(input, endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.CheckArc = input.ReadValueB32(endianess);
			this.CheckDistance = input.ReadValueB32(endianess);
			this.TargetFilter = BaseProperty.DeserializePropertyBitfield<TargetFilterType>(input, endianess);
			this.StickToTarget = input.ReadValueB32(endianess);
			this.UseExistingAutoTarget = input.ReadValueB32(endianess);
			this.UseReactionHitEvent = input.ReadValueB32(endianess);
			this.CharacterHeight = input.ReadValueF32(endianess);
			this.CharacterWidth = input.ReadValueF32(endianess);
			this.MinPriorityTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
			this.MaxPriorityTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
