using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11511572206409754781UL)]
	public class HeliLocoTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Acceleration { get; set; }
		public float Velocity { get; set; }
		public float ExtraFallingVelocity { get; set; }
		public float TurnAcceleration { get; set; }
		public float TurnVelocity { get; set; }
		public Vector Minimum { get; set; } = new Vector();
		public Vector Maximum { get; set; } = new Vector();
		public float TiltMax { get; set; }
		public float TiltDampingConstant { get; set; }
		public float TiltSpringConstant { get; set; }
		public float VelocityToAngleFactor { get; set; }
		public float MaxWashEffectHeight { get; set; }
		public float WashEffectFrequency { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.Velocity, endianess);
			output.WriteValueF32(this.ExtraFallingVelocity, endianess);
			output.WriteValueF32(this.TurnAcceleration, endianess);
			output.WriteValueF32(this.TurnVelocity, endianess);
			this.Minimum.Serialize(output, endianess);
			this.Maximum.Serialize(output, endianess);
			output.WriteValueF32(this.TiltMax, endianess);
			output.WriteValueF32(this.TiltDampingConstant, endianess);
			output.WriteValueF32(this.TiltSpringConstant, endianess);
			output.WriteValueF32(this.VelocityToAngleFactor, endianess);
			output.WriteValueF32(this.MaxWashEffectHeight, endianess);
			output.WriteValueF32(this.WashEffectFrequency, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Acceleration = input.ReadValueF32(endianess);
			this.Velocity = input.ReadValueF32(endianess);
			this.ExtraFallingVelocity = input.ReadValueF32(endianess);
			this.TurnAcceleration = input.ReadValueF32(endianess);
			this.TurnVelocity = input.ReadValueF32(endianess);
			this.Minimum.Deserialize(input, endianess);
			this.Maximum.Deserialize(input, endianess);
			this.TiltMax = input.ReadValueF32(endianess);
			this.TiltDampingConstant = input.ReadValueF32(endianess);
			this.TiltSpringConstant = input.ReadValueF32(endianess);
			this.VelocityToAngleFactor = input.ReadValueF32(endianess);
			this.MaxWashEffectHeight = input.ReadValueF32(endianess);
			this.WashEffectFrequency = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
