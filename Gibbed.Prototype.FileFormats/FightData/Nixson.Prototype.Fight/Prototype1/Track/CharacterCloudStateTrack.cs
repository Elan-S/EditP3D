using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(11524674574525033539UL)]
	public class CharacterCloudStateTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public CloudStateType State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<CloudStateType>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.State = BaseProperty.DeserializePropertyEnum<CloudStateType>(input, endianess);
		}
	}
}
