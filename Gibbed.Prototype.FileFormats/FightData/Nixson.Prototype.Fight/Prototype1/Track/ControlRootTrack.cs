using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(10183482681655297595UL)]
	public class ControlRootTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool MovementEnabled { get; set; }
		public float MaxSpeed { get; set; }
		public float Gravity { get; set; }
		public bool SteeringEnabled { get; set; }
		public float MaxTurnSpeed { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.MovementEnabled, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			output.WriteValueF32(this.Gravity, endianess);
			output.WriteValueB32(this.SteeringEnabled, endianess);
			output.WriteValueF32(this.MaxTurnSpeed, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MovementEnabled = input.ReadValueB32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.Gravity = input.ReadValueF32(endianess);
			this.SteeringEnabled = input.ReadValueB32(endianess);
			this.MaxTurnSpeed = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
