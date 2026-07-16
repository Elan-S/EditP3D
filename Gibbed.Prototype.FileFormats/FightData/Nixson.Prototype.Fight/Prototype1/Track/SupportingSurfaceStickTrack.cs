using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(10516540644062670056UL)]
	public class SupportingSurfaceStickTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public bool StickToSupportingSurface { get; set; }
		public bool ResetOrientationDelta { get; set; }
		public bool ResetLimbSmoothing { get; set; }
		public bool RestoreOnEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueB32(this.StickToSupportingSurface, endianess);
			output.WriteValueB32(this.ResetOrientationDelta, endianess);
			output.WriteValueB32(this.ResetLimbSmoothing, endianess);
			output.WriteValueB32(this.RestoreOnEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.StickToSupportingSurface = input.ReadValueB32(endianess);
			this.ResetOrientationDelta = input.ReadValueB32(endianess);
			this.ResetLimbSmoothing = input.ReadValueB32(endianess);
			this.RestoreOnEnd = input.ReadValueB32(endianess);
		}
	}
}
