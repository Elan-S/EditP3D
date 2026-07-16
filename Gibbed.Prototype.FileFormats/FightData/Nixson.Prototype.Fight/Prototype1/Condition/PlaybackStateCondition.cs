using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(15540816379440894602UL)]
	public class PlaybackStateCondition : P1Condition
	{
		public CompareOperator Compare { get; set; }
		public ulong State { get; set; }
		public ulong SpecificPlaybackSet { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<CompareOperator>(output, endianess, this.Compare);
			output.WriteValueU64(this.State, endianess);
			output.WriteValueU64(this.SpecificPlaybackSet, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Compare = BaseProperty.DeserializePropertyEnum<CompareOperator>(input, endianess);
			this.State = input.ReadValueU64(endianess);
			this.SpecificPlaybackSet = input.ReadValueU64(endianess);
		}
	}
}
