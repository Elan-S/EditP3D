using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.AvoidObstacles)]
	public class AvoidObstaclesTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public BranchReference MyBranch { get; set; } = new BranchReference();
		public bool GiveUpWithNoGapFound { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.MyBranch.Serialize(output, endianess);
			output.WriteValueB32(this.GiveUpWithNoGapFound, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MyBranch.Deserialize(input, endianess);
			this.GiveUpWithNoGapFound = input.ReadValueB32(endianess);
		}
	}
}
