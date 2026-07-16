using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(14646807593065489617UL)]
	public class FlyingDashTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityAdd { get; set; }
		public float VelocityMax { get; set; }
		public bool PreserveYVelocity { get; set; }
		public ulong DashToTargetGrabbableClass { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityAdd, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueB32(this.PreserveYVelocity, endianess);
			output.WriteValueU64(this.DashToTargetGrabbableClass, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityAdd = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.PreserveYVelocity = input.ReadValueB32(endianess);
			this.DashToTargetGrabbableClass = input.ReadValueU64(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
		}
	}
}
