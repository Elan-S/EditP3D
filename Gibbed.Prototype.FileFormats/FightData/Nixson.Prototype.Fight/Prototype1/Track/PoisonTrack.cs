using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Poison)]
	public class PoisonTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public AttackType PoisonType { get; set; }
		public float DamageRate { get; set; }
		public float DamageSpikes { get; set; }
		public float TimeToHit { get; set; }
		public float ExtraRandomTime { get; set; }
		public int Priority { get; set; }
		public PoisonTrack.WhoType Who { get; set; }
		public ulong GrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<AttackType>(output, endianess, this.PoisonType);
			output.WriteValueF32(this.DamageRate, endianess);
			output.WriteValueF32(this.DamageSpikes, endianess);
			output.WriteValueF32(this.TimeToHit, endianess);
			output.WriteValueF32(this.ExtraRandomTime, endianess);
			output.WriteValueS32(this.Priority, endianess);
			BaseProperty.SerializePropertyEnum<PoisonTrack.WhoType>(output, endianess, this.Who);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.PoisonType = BaseProperty.DeserializePropertyEnum<AttackType>(input, endianess);
			this.DamageRate = input.ReadValueF32(endianess);
			this.DamageSpikes = input.ReadValueF32(endianess);
			this.TimeToHit = input.ReadValueF32(endianess);
			this.ExtraRandomTime = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.Who = BaseProperty.DeserializePropertyEnum<PoisonTrack.WhoType>(input, endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
		public enum WhoType : ulong
		{
			Self = 23429428375025418UL,
			Target = 856854631462190855UL,
			GrabSlot = 7882620167033900854UL,
			GrabTarget = 1754404701201221985UL
		}
	}
}
