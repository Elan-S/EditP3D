using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(15542058887873141646UL)]
	public class TimeOfDayBlendTrack : P1Track
	{
		public ulong GroupName { get; set; }
		public ulong VariationName { get; set; }
		public float FadeIn { get; set; }
		public float Duration { get; set; }
		public float FadeOut { get; set; }
		public TimeOfDayBlendTrack.LevelHash Level { get; set; }
		public bool Sky { get; set; }
		public bool GlobalLighting { get; set; }
		public bool Lighting { get; set; }
		public bool Bloom { get; set; }
		public bool ColourMatrix { get; set; }
		public bool Shaders { get; set; }
		public bool Clouds { get; set; }
		public bool Fogs { get; set; }
		public bool Abortable { get; set; }
		public bool DynamicDuration { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU64(this.GroupName, endianess);
			output.WriteValueU64(this.VariationName, endianess);
			output.WriteValueF32(this.FadeIn, endianess);
			output.WriteValueF32(this.Duration, endianess);
			output.WriteValueF32(this.FadeOut, endianess);
			BaseProperty.SerializePropertyEnum<TimeOfDayBlendTrack.LevelHash>(output, endianess, this.Level);
			output.WriteValueB32(this.Sky, endianess);
			output.WriteValueB32(this.GlobalLighting, endianess);
			output.WriteValueB32(this.Lighting, endianess);
			output.WriteValueB32(this.Bloom, endianess);
			output.WriteValueB32(this.ColourMatrix, endianess);
			output.WriteValueB32(this.Shaders, endianess);
			output.WriteValueB32(this.Clouds, endianess);
			output.WriteValueB32(this.Fogs, endianess);
			output.WriteValueB32(this.Abortable, endianess);
			output.WriteValueB32(this.DynamicDuration, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.GroupName = input.ReadValueU64(endianess);
			this.VariationName = input.ReadValueU64(endianess);
			this.FadeIn = input.ReadValueF32(endianess);
			this.Duration = input.ReadValueF32(endianess);
			this.FadeOut = input.ReadValueF32(endianess);
			this.Level = BaseProperty.DeserializePropertyEnum<TimeOfDayBlendTrack.LevelHash>(input, endianess);
			this.Sky = input.ReadValueB32(endianess);
			this.GlobalLighting = input.ReadValueB32(endianess);
			this.Lighting = input.ReadValueB32(endianess);
			this.Bloom = input.ReadValueB32(endianess);
			this.ColourMatrix = input.ReadValueB32(endianess);
			this.Shaders = input.ReadValueB32(endianess);
			this.Clouds = input.ReadValueB32(endianess);
			this.Fogs = input.ReadValueB32(endianess);
			this.Abortable = input.ReadValueB32(endianess);
			this.DynamicDuration = input.ReadValueB32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
		}
		public enum LevelHash : ulong
		{
			AtmosphereLevel = 4344160273227135606UL,
			FXLevel = 13433670759597913104UL,
			PowersLevel = 1642107883378848372UL
		}
	}
}
