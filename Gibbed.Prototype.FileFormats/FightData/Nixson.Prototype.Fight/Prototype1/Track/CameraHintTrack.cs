using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.CameraHint)]
	public class CameraHintTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public CameraHintTrack.HintType Hint { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<CameraHintTrack.HintType>(output, endianess, this.Hint);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Hint = BaseProperty.DeserializePropertyEnum<CameraHintTrack.HintType>(input, endianess);
		}
		public enum HintType : ulong
		{
			Slide = 5865338372162726487UL,
			Falling = 1546725305262767233UL,
			Running = 13391260477670660353UL,
			Jumping = 6370575284602695520UL,
			Climbing = 5229273973122016849UL,
			WallRun = 7764074208662850059UL,
			Float = 4980286969596303762UL,
			QuickTurn = 2669843167633621244UL,
			HopJump = 11894878010327071993UL,
			Parkour = 13938991474149399574UL,
			Grapple = 4313919289786887845UL,
			RollLeft = 11461592797946620918UL,
			RollRight = 16208179408930410493UL,
			AutoTarget = 4751627266579880038UL,
			Glide = 5015188191654984259UL,
			AirDash = 11955892389761813344UL,
			LockControls = 9256397209407139691UL
		}
	}
}
