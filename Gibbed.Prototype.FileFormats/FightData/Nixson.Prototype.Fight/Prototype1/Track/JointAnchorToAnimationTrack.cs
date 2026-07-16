using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(9746956955326011222UL)]
	public class JointAnchorToAnimationTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Joint { get; set; }
		public ulong Animation { get; set; }
		public float Frame { get; set; }
		public int Priority { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueU64(this.Animation, endianess);
			output.WriteValueF32(this.Frame, endianess);
			output.WriteValueS32(this.Priority, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.Animation = input.ReadValueU64(endianess);
			this.Frame = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
		}
	}
}
