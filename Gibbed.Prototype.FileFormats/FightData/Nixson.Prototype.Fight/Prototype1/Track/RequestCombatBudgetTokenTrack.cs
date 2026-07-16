using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(9442929935869415691UL)]
	public class RequestCombatBudgetTokenTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public RequestCombatBudgetTokenTrack.CombatEnumType CombatType { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<RequestCombatBudgetTokenTrack.CombatEnumType>(output, endianess, this.CombatType);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.CombatType = BaseProperty.DeserializePropertyEnum<RequestCombatBudgetTokenTrack.CombatEnumType>(input, endianess);
		}
		public enum CombatEnumType : ulong
		{
			Bullet = 1071813355822642564UL,
			Rocket = 14395127206582377560UL,
			Explosion = 6119690315119812053UL,
			Gib = 305522356882UL
		}
	}
}
