using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.ThreatBall)]
	public class ThreatBallTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public float Radius { get; set; }
		public float TimeToLive { get; set; }
		public ulong Joint { get; set; }
		public ulong ThreatName { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.TimeToLive, endianess);
			output.WriteValueU64(this.Joint, endianess);
			output.WriteValueU64(this.ThreatName, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Offset = new Vector(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.TimeToLive = input.ReadValueF32(endianess);
			this.Joint = input.ReadValueU64(endianess);
			this.ThreatName = input.ReadValueU64(endianess);
		}
	}
}
