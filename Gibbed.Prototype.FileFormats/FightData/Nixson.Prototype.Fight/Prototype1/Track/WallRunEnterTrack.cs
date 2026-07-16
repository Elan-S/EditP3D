using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14876403155400372803UL)]
	public class WallRunEnterTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float TimeCancel { get; set; }
		public float VelocityXZMin { get; set; }
		public float VelocityXZMax { get; set; }
		public float VelocityUpMin { get; set; }
		public float VelocityUpMax { get; set; }
		public float TurningVelocityMin { get; set; }
		public float TurningVelocityMax { get; set; }
		public float TurningAccelerationMin { get; set; }
		public float TurningAccelerationMax { get; set; }
		public float SurfaceDistance { get; set; }
		public float SurfacePushoutVelocityMax { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public BranchReference Branch { get; set; } = new BranchReference();
		public int InterruptPriority { get; set; }
		public PropertyConditionGroup Conditions { get; set; } = new PropertyConditionGroup(PropertyHash.Conditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.TimeCancel, endianess);
			output.WriteValueF32(this.VelocityXZMin, endianess);
			output.WriteValueF32(this.VelocityXZMax, endianess);
			output.WriteValueF32(this.VelocityUpMin, endianess);
			output.WriteValueF32(this.VelocityUpMax, endianess);
			output.WriteValueF32(this.TurningVelocityMin, endianess);
			output.WriteValueF32(this.TurningVelocityMax, endianess);
			output.WriteValueF32(this.TurningAccelerationMin, endianess);
			output.WriteValueF32(this.TurningAccelerationMax, endianess);
			output.WriteValueF32(this.SurfaceDistance, endianess);
			output.WriteValueF32(this.SurfacePushoutVelocityMax, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			this.Branch.Serialize(output, endianess);
			output.WriteValueS32(this.InterruptPriority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.TimeCancel = input.ReadValueF32(endianess);
			this.VelocityXZMin = input.ReadValueF32(endianess);
			this.VelocityXZMax = input.ReadValueF32(endianess);
			this.VelocityUpMin = input.ReadValueF32(endianess);
			this.VelocityUpMax = input.ReadValueF32(endianess);
			this.TurningVelocityMin = input.ReadValueF32(endianess);
			this.TurningVelocityMax = input.ReadValueF32(endianess);
			this.TurningAccelerationMin = input.ReadValueF32(endianess);
			this.TurningAccelerationMax = input.ReadValueF32(endianess);
			this.SurfaceDistance = input.ReadValueF32(endianess);
			this.SurfacePushoutVelocityMax = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.Branch = new BranchReference(input, endianess);
			this.InterruptPriority = input.ReadValueS32(endianess);
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
