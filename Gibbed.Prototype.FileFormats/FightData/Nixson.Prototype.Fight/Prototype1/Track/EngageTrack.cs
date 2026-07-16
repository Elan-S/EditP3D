using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.Engage)]
	public class EngageTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MinStopDistance { get; set; }
		public float MaxStopDistance { get; set; }
		public float ExtraMoveDistance { get; set; }
		public float MinTimeInPosition { get; set; }
		public float MaxTimeInPosition { get; set; }
		public float MaxTimeMelee { get; set; }
		public float KneelingProbability { get; set; }
		public float MinSpeed { get; set; }
		public float MaxSpeed { get; set; }
		public ulong WeaponGrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MinStopDistance, endianess);
			output.WriteValueF32(this.MaxStopDistance, endianess);
			output.WriteValueF32(this.ExtraMoveDistance, endianess);
			output.WriteValueF32(this.MinTimeInPosition, endianess);
			output.WriteValueF32(this.MaxTimeInPosition, endianess);
			output.WriteValueF32(this.MaxTimeMelee, endianess);
			output.WriteValueF32(this.KneelingProbability, endianess);
			output.WriteValueF32(this.MinSpeed, endianess);
			output.WriteValueF32(this.MaxSpeed, endianess);
			output.WriteValueU64(this.WeaponGrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MinStopDistance = input.ReadValueF32(endianess);
			this.MaxStopDistance = input.ReadValueF32(endianess);
			this.ExtraMoveDistance = input.ReadValueF32(endianess);
			this.MinTimeInPosition = input.ReadValueF32(endianess);
			this.MaxTimeInPosition = input.ReadValueF32(endianess);
			this.MaxTimeMelee = input.ReadValueF32(endianess);
			this.KneelingProbability = input.ReadValueF32(endianess);
			this.MinSpeed = input.ReadValueF32(endianess);
			this.MaxSpeed = input.ReadValueF32(endianess);
			this.WeaponGrabSlot = input.ReadValueU64(endianess);
		}
	}
}
