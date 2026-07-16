using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Joint)]
	public class JointTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong Joint { get; set; }
		public bool UseRestPose { get; set; }
		public bool HasTranslation { get; set; }
		public Vector Translation { get; set; } = new Vector();
		public bool HasRotation { get; set; }
		public Vector Rotation { get; set; } = new Vector();
		public int Priority { get; set; }
		public bool AutomaticBlendTimes { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueB32(this.UseRestPose, endianess);
			output.WriteValueB32(this.HasTranslation, endianess);
			this.Translation.Serialize(output, endianess);
			output.WriteValueB32(this.HasRotation, endianess);
			this.Rotation.Serialize(output, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueB32(this.AutomaticBlendTimes, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.UseRestPose = input.ReadValueB32(endianess);
			this.HasTranslation = input.ReadValueB32(endianess);
			this.Translation = new Vector(input, endianess);
			this.HasRotation = input.ReadValueB32(endianess);
			this.Rotation = new Vector(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.AutomaticBlendTimes = input.ReadValueB32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
