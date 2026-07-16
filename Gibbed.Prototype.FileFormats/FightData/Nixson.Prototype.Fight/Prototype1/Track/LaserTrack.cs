using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Laser)]
	public class LaserTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Shader { get; set; }
		public float Thickness { get; set; }
		public Color Colour { get; set; } = new Color();
		public ulong MyJoint { get; set; }
		public Vector MyOffset { get; set; } = new Vector();
		public ulong TargetJoint { get; set; }
		public Vector TargetOffset { get; set; } = new Vector();
		public float TrackingSpeed { get; set; }
		public float LongitudinalOffset { get; set; }
		public float PerpendicularOffset { get; set; }
		public float PerpendicularFinalOffset { get; set; }
		public float PerpendicularFinalTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Shader, endianess);
			output.WriteValueF32(this.Thickness, endianess);
			this.Colour.Serialize(output, endianess);
			output.WriteValueU64(this.MyJoint, endianess);
			this.MyOffset.Serialize(output, endianess);
			output.WriteValueU64(this.TargetJoint, endianess);
			this.TargetOffset.Serialize(output, endianess);
			output.WriteValueF32(this.TrackingSpeed, endianess);
			output.WriteValueF32(this.LongitudinalOffset, endianess);
			output.WriteValueF32(this.PerpendicularOffset, endianess);
			output.WriteValueF32(this.PerpendicularFinalOffset, endianess);
			output.WriteValueF32(this.PerpendicularFinalTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Shader = input.ReadValueU64(endianess);
			this.Thickness = input.ReadValueF32(endianess);
			this.Colour.Deserialize(input, endianess);
			this.MyJoint = input.ReadValueU64(endianess);
			this.MyOffset.Deserialize(input, endianess);
			this.TargetJoint = input.ReadValueU64(endianess);
			this.TargetOffset.Deserialize(input, endianess);
			this.TrackingSpeed = input.ReadValueF32(endianess);
			this.LongitudinalOffset = input.ReadValueF32(endianess);
			this.PerpendicularOffset = input.ReadValueF32(endianess);
			this.PerpendicularFinalOffset = input.ReadValueF32(endianess);
			this.PerpendicularFinalTime = input.ReadValueF32(endianess);
		}
	}
}
