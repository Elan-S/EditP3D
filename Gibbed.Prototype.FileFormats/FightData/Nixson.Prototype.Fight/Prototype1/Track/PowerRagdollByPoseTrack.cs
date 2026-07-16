using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.PowerRagdollByPose)]
	public class PowerRagdollByPoseTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ulong PoseAnimName { get; set; }
		public float AnimFrame { get; set; }
		public ulong ConstraintPropertiesName { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueU64(this.PoseAnimName, endianess);
			output.WriteValueF32(this.AnimFrame, endianess);
			output.WriteValueU64(this.ConstraintPropertiesName, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.PoseAnimName = input.ReadValueU64(endianess);
			this.AnimFrame = input.ReadValueF32(endianess);
			this.ConstraintPropertiesName = input.ReadValueU64(endianess);
		}
	}
}
