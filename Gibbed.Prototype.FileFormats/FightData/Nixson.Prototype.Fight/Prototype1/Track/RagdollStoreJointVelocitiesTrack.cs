using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(16597962693944688149UL)]
	public class RagdollStoreJointVelocitiesTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
		}
	}
}
