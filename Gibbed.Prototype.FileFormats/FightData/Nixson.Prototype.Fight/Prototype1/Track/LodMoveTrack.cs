using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(9968373683988398674UL)]
	public class LodMoveTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public LODStateType Walk_anim { get; set; }
		public LODStateType Idle_anim { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<LODStateType>(output, endianess, this.Walk_anim);
			BaseProperty.SerializePropertyEnum<LODStateType>(output, endianess, this.Idle_anim);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Walk_anim = BaseProperty.DeserializePropertyEnum<LODStateType>(input, endianess);
			this.Idle_anim = BaseProperty.DeserializePropertyEnum<LODStateType>(input, endianess);
		}
	}
}
