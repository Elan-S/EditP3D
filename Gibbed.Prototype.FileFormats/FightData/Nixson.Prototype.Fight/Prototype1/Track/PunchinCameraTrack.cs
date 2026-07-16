using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(9938684566957480724UL)]
	public class PunchinCameraTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float InTime { get; set; }
		public float OutTime { get; set; }
		public float Dolly { get; set; }
		public float Zoom { get; set; }
		public bool ReAim { get; set; }
		public float VectorX { get; set; }
		public float VectorY { get; set; }
		public float VectorZ { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.InTime, endianess);
			output.WriteValueF32(this.OutTime, endianess);
			output.WriteValueF32(this.Dolly, endianess);
			output.WriteValueF32(this.Zoom, endianess);
			output.WriteValueB32(this.ReAim, endianess);
			output.WriteValueF32(this.VectorX, endianess);
			output.WriteValueF32(this.VectorY, endianess);
			output.WriteValueF32(this.VectorZ, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.InTime = input.ReadValueF32(endianess);
			this.OutTime = input.ReadValueF32(endianess);
			this.Dolly = input.ReadValueF32(endianess);
			this.Zoom = input.ReadValueF32(endianess);
			this.ReAim = input.ReadValueB32(endianess);
			this.VectorX = input.ReadValueF32(endianess);
			this.VectorY = input.ReadValueF32(endianess);
			this.VectorZ = input.ReadValueF32(endianess);
		}
	}
}
