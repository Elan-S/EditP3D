using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.ShootAutoTarget)]
	public class ShootAutoTargetTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Arc { get; set; }
		public float MaxDistance { get; set; }
		public bool UseSecondCone { get; set; }
		public float Arc2 { get; set; }
		public float MaxDistance2 { get; set; }
		public TargetClass MinPriorityTargetClass { get; set; }
		public TargetClass MaxPriorityTargetClass { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Arc, endianess);
			output.WriteValueF32(this.MaxDistance, endianess);
			output.WriteValueB32(this.UseSecondCone, endianess);
			output.WriteValueF32(this.Arc2, endianess);
			output.WriteValueF32(this.MaxDistance2, endianess);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.MinPriorityTargetClass);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.MaxPriorityTargetClass);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Arc = input.ReadValueF32(endianess);
			this.MaxDistance = input.ReadValueF32(endianess);
			this.UseSecondCone = input.ReadValueB32(endianess);
			this.Arc2 = input.ReadValueF32(endianess);
			this.MaxDistance2 = input.ReadValueF32(endianess);
			this.MinPriorityTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
			this.MaxPriorityTargetClass = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
		}
	}
}
