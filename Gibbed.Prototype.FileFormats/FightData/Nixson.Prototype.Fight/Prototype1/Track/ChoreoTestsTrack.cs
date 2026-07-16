using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.ChoreoTests)]
	public class ChoreoTestsTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool TestRandomSteer { get; set; }
		public bool TestPoseDriver { get; set; }
		public bool TestRestPoseDriver { get; set; }
		public bool TestDriverCopy { get; set; }
		public ulong TestDriverCopySource { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.TestRandomSteer, endianess);
			output.WriteValueB32(this.TestPoseDriver, endianess);
			output.WriteValueB32(this.TestRestPoseDriver, endianess);
			output.WriteValueB32(this.TestDriverCopy, endianess);
			output.WriteValueU64(this.TestDriverCopySource, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.TestRandomSteer = input.ReadValueB32(endianess);
			this.TestPoseDriver = input.ReadValueB32(endianess);
			this.TestRestPoseDriver = input.ReadValueB32(endianess);
			this.TestDriverCopy = input.ReadValueB32(endianess);
			this.TestDriverCopySource = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
