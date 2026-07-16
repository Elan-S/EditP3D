using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(11265396890264057221UL)]
	public class CharacterIKTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong RootJoint { get; set; }
		public ulong MidJoint { get; set; }
		public ulong EndJoint { get; set; }
		public TargetClass TargetType { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.RootJoint, endianess);
			output.WriteValueU64(this.MidJoint, endianess);
			output.WriteValueU64(this.EndJoint, endianess);
			BaseProperty.SerializePropertyEnum<TargetClass>(output, endianess, this.TargetType);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.RootJoint = input.ReadValueU64(endianess);
			this.MidJoint = input.ReadValueU64(endianess);
			this.EndJoint = input.ReadValueU64(endianess);
			this.TargetType = BaseProperty.DeserializePropertyEnum<TargetClass>(input, endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
