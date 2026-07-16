using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(11524674574525033539UL)]
	public class CharacterCloudStateCondition : P1Condition
	{
		public bool Match { get; set; }
		public CloudStateType State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Match, endianess);
			BaseProperty.SerializePropertyEnum<CloudStateType>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Match = input.ReadValueB32(endianess);
			this.State = BaseProperty.DeserializePropertyEnum<CloudStateType>(input, endianess);
		}
	}
}
