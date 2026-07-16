using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(9948563989609133665UL)]
	public class ZiplineTakeOffTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public Vector ZiplineSpeed { get; set; } = new Vector();
		public float ZiplineTime { get; set; }
		public Vector CharacterSpeed { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			this.ZiplineSpeed.Serialize(output, endianess);
			output.WriteValueF32(this.ZiplineTime, endianess);
			this.CharacterSpeed.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ZiplineSpeed.Deserialize(input, endianess);
			this.ZiplineTime = input.ReadValueF32(endianess);
			this.CharacterSpeed.Deserialize(input, endianess);
		}
	}
}
