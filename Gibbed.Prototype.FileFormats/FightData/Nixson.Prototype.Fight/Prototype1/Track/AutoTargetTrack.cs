using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(15089281274414485574UL)]
	public class AutoTargetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float TurningVelocity { get; set; }
		public bool UseInput { get; set; }
		public bool UseCamera { get; set; }
		public Vector Direction { get; set; } = new Vector();
		public float Arc { get; set; }
		public float MaxDistance { get; set; }
		public bool CheckArc { get; set; }
		public bool CheckDistance { get; set; }
		public float OrientOffsetFromTarget { get; set; }
		public TargetFilterType TargetFilter { get; set; }
		public AutoTargetTrack.AutoTargetType TargetOverrides { get; set; }
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
			output.WriteValueF32(this.TurningVelocity, endianess);
			output.WriteValueB32(this.UseInput, endianess);
			output.WriteValueB32(this.UseCamera, endianess);
			this.Direction.Serialize(output, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueB32(this.CheckArc, endianess);
			output.WriteValueB32(this.CheckDistance, endianess);
			output.WriteValueF32(this.OrientOffsetFromTarget, endianess);
			BaseProperty.SerializePropertyBitfield<TargetFilterType>(output, endianess, this.TargetFilter);
			BaseProperty.SerializePropertyBitfield<AutoTargetTrack.AutoTargetType>(output, endianess, this.TargetOverrides);
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
			this.TurningVelocity = input.ReadValueF32(endianess);
			this.UseInput = input.ReadValueB32(endianess);
			this.UseCamera = input.ReadValueB32(endianess);
			this.Direction = new Vector(input, endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.CheckArc = input.ReadValueB32(endianess);
			this.CheckDistance = input.ReadValueB32(endianess);
			this.OrientOffsetFromTarget = input.ReadValueF32(endianess);
			this.TargetFilter = BaseProperty.DeserializePropertyBitfield<TargetFilterType>(input, endianess);
			this.TargetOverrides = BaseProperty.DeserializePropertyBitfield<AutoTargetTrack.AutoTargetType>(input, endianess);
			this.MinPriorityTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
			this.MaxPriorityTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
		[Flags]
		public enum AutoTargetType : ulong
		{
			ExistingAutoTarget = 1UL,
			DetectedObject = 2UL,
			ReactionGiver = 4UL,
			PowerTarget = 8UL,
			ForceExistingTarget = 16UL
		}
	}
}
