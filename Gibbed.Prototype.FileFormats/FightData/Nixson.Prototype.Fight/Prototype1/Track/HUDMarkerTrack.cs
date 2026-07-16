using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.HUDMarker)]
	public class HUDMarkerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public MarkerType Type { get; set; }
		public HUDMarkerTrack.HUDDisplayType DisplayType { get; set; }
		public HUDMarkerTrack.HUDIconAnimation AnimType { get; set; }
		public HUDMarkerTrack.HUDIconColor Color { get; set; }
		public float Radius { get; set; }
		public bool UseGrabSlot { get; set; }
		public ulong GrabSlot { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			BaseProperty.SerializePropertyEnum<MarkerType>(output, endianess, this.Type);
			BaseProperty.SerializePropertyBitfield<HUDMarkerTrack.HUDDisplayType>(output, endianess, this.DisplayType);
			BaseProperty.SerializePropertyEnum<HUDMarkerTrack.HUDIconAnimation>(output, endianess, this.AnimType);
			BaseProperty.SerializePropertyEnum<HUDMarkerTrack.HUDIconColor>(output, endianess, this.Color);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueB32(this.UseGrabSlot, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Type = BaseProperty.DeserializePropertyEnum<MarkerType>(input, endianess);
			this.DisplayType = BaseProperty.DeserializePropertyBitfield<HUDMarkerTrack.HUDDisplayType>(input, endianess);
			this.AnimType = BaseProperty.DeserializePropertyEnum<HUDMarkerTrack.HUDIconAnimation>(input, endianess);
			this.Color = BaseProperty.DeserializePropertyEnum<HUDMarkerTrack.HUDIconColor>(input, endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.UseGrabSlot = input.ReadValueB32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
		}
		public enum HUDIconAnimation : ulong
		{
			NONE = 22018748215728118UL,
			PULSE_SLOW = 3665480714829312421UL,
			PULSE_FAST = 3668585279254091496UL,
			INTRO = 5167983425046386766UL,
			OUTRO = 5589952664214003225UL
		}
		public enum HUDIconColor : ulong
		{
			EVENT = 4879330626040897096UL,
			OBJECTIVE = 17058728716782777749UL,
			MISSION = 4334989616241774622UL,
			OPPORTUNITY = 1370945567035357059UL,
			SCIENTIST = 5005180118806087990UL,
			META_GAME = 16428558314997069388UL,
			ZONE_INFECTED = 17595790134620054805UL,
			ZONE_MILITARY = 10616222432864268188UL,
			AWARENESS_PERCEPTION = 17801449295885656871UL,
			AWARENESS_LOW = 6576193518582633164UL,
			AWARENESS_MED_SOUND = 12045037681404148444UL,
			AWARENESS_MED = 6576193522894783440UL,
			AWARENESS_HIGH = 14607332645658772330UL,
			AWARENESS_HIGH_OUTOFSIGHT = 8428198859817923579UL,
			STRIKE_TEAM = 13141943391753985174UL,
			BASE = 18631255110497579UL,
			HIVE = 20324945720908898UL,
			MISSION_AREA = 16690139112599310638UL,
			MEDAL_NONE = 1590293401304588950UL,
			MEDAL_BRONZE = 12260960288387540UL,
			MEDAL_SILVER = 15409522211401927059UL,
			MEDAL_GOLD = 1588317879328936414UL,
			MEDAL_PLATINUM = 7872818629347817308UL,
			DESTROY = 8991619527592467022UL
		}
		[Flags]
		public enum HUDDisplayType : ulong
		{
			Mainmap = 1UL,
			Minimap = 2UL,
			Onscreen = 4UL,
			Offscreen = 8UL
		}
	}
}
