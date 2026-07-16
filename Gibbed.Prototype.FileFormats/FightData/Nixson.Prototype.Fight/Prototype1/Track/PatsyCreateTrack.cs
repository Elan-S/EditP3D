using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.PatsyCreate)]
	public class PatsyCreateTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public PatsyCreateTrack.WhoType Who { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<PatsyCreateTrack.WhoType>(output, endianess, this.Who);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Who = BaseProperty.DeserializePropertyEnum<PatsyCreateTrack.WhoType>(input, endianess);
		}
		public enum WhoType : ulong
		{
			Target = 856854631462190855UL,
			AutoTarget = 4751627266579880038UL,
			Holding = 10464599244845980941UL,
			Hit = 309834113563UL,
			Clear = 4728793748633909019UL
		}
	}
}
