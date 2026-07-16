using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.SmartHintSetTimer)]
	public class SmartHintSetTimerTrack : P1Track
	{
		public SmartHintSetTimerTrack.TimerType Timer { get; set; }
		public float Value { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<SmartHintSetTimerTrack.TimerType>(output, endianess, this.Timer);
			output.WriteValueF32(this.Value, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Timer = BaseProperty.DeserializePropertyEnum<SmartHintSetTimerTrack.TimerType>(input, endianess);
			this.Value = input.ReadValueF32(endianess);
		}
		public enum TimerType : ulong
		{
			Sprint = 139431196041208856UL,
			WallRun = 7764074208662850059UL,
			Target = 856854631462190855UL,
			Special = 14665729337295026497UL,
			Attack = 17648781240126830036UL,
			ThrowObject = 10411039871081615289UL,
			CameraStick = 12012351502380158027UL,
			CameraReset = 12121494932376928046UL,
			Combo = 4729075459507432078UL,
			AttackCharged = 9166767161979886648UL,
			JumpCharged = 12737234338093086820UL,
			JumpChargedSprinting = 680685272192896404UL,
			JumpMulti = 2163982626737384203UL,
			CameraResetInTank = 6622348168307747967UL,
			Consume = 1055741811302362110UL,
			EPRemind = 738561241418108496UL,
			PowerUnused = 2129866768912735281UL,
			ThermalVisionUnused = 1638144665266917203UL,
			AirDash = 11955892389761813344UL,
			WOINeglected = 14745935532602829756UL,
			GlideRemind = 9189042604311415864UL,
			StartMission = 4351880290315119674UL,
			WeaponGrab = 18395871681580613670UL,
			Devastator = 16626996011609549067UL
		}
	}
}
