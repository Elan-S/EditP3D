using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.Health)]
	public class HealthTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public OperationType Operation { get; set; }
		public HealthTrack.ApplyToHealhType ApplyTo { get; set; }
		public float Health { get; set; }
		public bool ChangeMaxHealth { get; set; }
		public bool ChangeMinHealth { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyEnum<OperationType>(output, endianess, this.Operation);
			BaseProperty.SerializePropertyEnum<HealthTrack.ApplyToHealhType>(output, endianess, this.ApplyTo);
			output.WriteValueF32(this.Health, endianess);
			output.WriteValueB32(this.ChangeMaxHealth, endianess);
			output.WriteValueB32(this.ChangeMinHealth, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Operation = BaseProperty.DeserializePropertyEnum<OperationType>(input, endianess);
			this.ApplyTo = BaseProperty.DeserializePropertyEnum<HealthTrack.ApplyToHealhType>(input, endianess);
			this.Health = input.ReadValueF32(endianess);
			this.ChangeMaxHealth = input.ReadValueB32(endianess);
			this.ChangeMinHealth = input.ReadValueB32(endianess);
		}
		public enum ApplyToHealhType : ulong
		{
			CurrentHealth = 14120912836564576008UL,
			PercentageCurrentHealth = 5073460416179252554UL,
			PercentageMaximumHealth = 11308088312728993153UL
		}
	}
}
