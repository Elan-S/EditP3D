using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(14733191237556582384UL)]
	public class LODSteerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Speed { get; set; }
		public LODSteerTrack.AnimType Walk_anim { get; set; }
		public LODSteerTrack.AnimType Idle_anim { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Speed, endianess);
			BaseProperty.SerializePropertyEnum<LODSteerTrack.AnimType>(output, endianess, this.Walk_anim);
			BaseProperty.SerializePropertyEnum<LODSteerTrack.AnimType>(output, endianess, this.Idle_anim);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Speed = input.ReadValueF32(endianess);
			this.Walk_anim = BaseProperty.DeserializePropertyEnum<LODSteerTrack.AnimType>(input, endianess);
			this.Idle_anim = BaseProperty.DeserializePropertyEnum<LODSteerTrack.AnimType>(input, endianess);
		}
		public enum AnimType : ulong
		{
			PPS_NONE = 17576375266414389028UL,
			PPS_STANDING = 14839415451792732434UL,
			PPS_WALKING = 6640379199237813563UL,
			PPS_RUNNING = 5574202178697275883UL,
			PPS_FLOAT = 16359170657376327024UL,
			PPS_DEAD = 17579198064016454736UL,
			PPS_COWER = 16754144050052644138UL,
			PPS_LOWMORALEWALKING = 14802021181383392899UL,
			PPS_LOWMORALERUNNING = 6384415947139926867UL,
			PPS_LOWMORALESTANDING = 6079132282527946170UL,
			PPS_SPAWN_DEAD = 1598921015234818384UL,
			PPS_RUBBERNECKING = 5362461859085663867UL
		}
	}
}
