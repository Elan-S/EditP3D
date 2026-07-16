using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(13622579821819661655UL)]
	public class SuckIntoPlayerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float InitialVelocity { get; set; }
		public float MaxVelocity { get; set; }
		public float Acceleration { get; set; }
		public float FadeDistStart { get; set; }
		public float ScaleDistStart { get; set; }
		public float ScaleEnd { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public BranchReference WhenFinished { get; set; } = new BranchReference();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.InitialVelocity, endianess);
			output.WriteValueF32(this.MaxVelocity, endianess);
			output.WriteValueF32(this.Acceleration, endianess);
			output.WriteValueF32(this.FadeDistStart, endianess);
			output.WriteValueF32(this.ScaleDistStart, endianess);
			output.WriteValueF32(this.ScaleEnd, endianess);
			this.Offset.Serialize(output, endianess);
			this.WhenFinished.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.InitialVelocity = input.ReadValueF32(endianess);
			this.MaxVelocity = input.ReadValueF32(endianess);
			this.Acceleration = input.ReadValueF32(endianess);
			this.FadeDistStart = input.ReadValueF32(endianess);
			this.ScaleDistStart = input.ReadValueF32(endianess);
			this.ScaleEnd = input.ReadValueF32(endianess);
			this.Offset.Deserialize(input, endianess);
			this.WhenFinished.Deserialize(input, endianess);
		}
	}
}
