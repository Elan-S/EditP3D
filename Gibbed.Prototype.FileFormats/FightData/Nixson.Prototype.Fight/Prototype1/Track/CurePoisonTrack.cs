using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12353969839515970191UL)]
	public class CurePoisonTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public AttackType PoisonType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.PoisonType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.PoisonType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
		}
	}
}
