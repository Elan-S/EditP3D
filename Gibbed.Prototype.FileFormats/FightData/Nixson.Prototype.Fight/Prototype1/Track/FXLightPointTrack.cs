using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.FXLightPoint)]
	public class FXLightPointTrack : P1Track
	{
		public bool AbortWhenInterrupted { get; set; }
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public ulong ParentObjectName { get; set; }
		public float Wait { get; set; }
		public float FadeIn { get; set; }
		public float Duration { get; set; }
		public float FadeOut { get; set; }
		public float WaitRand { get; set; }
		public float FadeInRand { get; set; }
		public float DurationRand { get; set; }
		public float FadeOutRand { get; set; }
		public float Intensity { get; set; }
		public float Radius { get; set; }
		public float IntensityRand { get; set; }
		public float RadiusRand { get; set; }
		public float FlickerIntensityFreq { get; set; }
		public float FlickerIntensityFreqRand { get; set; }
		public float FlickerIntensityMag { get; set; }
		public float FlickerIntensityMagRand { get; set; }
		public float FlickerRadiusFreq { get; set; }
		public float FlickerRadiusFreqRand { get; set; }
		public float FlickerRadiusMag { get; set; }
		public float FlickerRadiusMagRand { get; set; }
		public Color StartColour { get; set; } = new Color();
		public Color EndColour { get; set; } = new Color();
		public SpaceType EmitSpace { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public SpaceType AttachSpace { get; set; }
		public ulong AttachJoint { get; set; }
		public bool Falloff { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.AbortWhenInterrupted, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.ParentObjectName, endianess);
			output.WriteValueF32(this.Wait, endianess);
			output.WriteValueF32(this.FadeIn, endianess);
			output.WriteValueF32(this.Duration, endianess);
			output.WriteValueF32(this.FadeOut, endianess);
			output.WriteValueF32(this.WaitRand, endianess);
			output.WriteValueF32(this.FadeInRand, endianess);
			output.WriteValueF32(this.DurationRand, endianess);
			output.WriteValueF32(this.FadeOutRand, endianess);
			output.WriteValueF32(this.Intensity, endianess);
			output.WriteValueF32(this.Radius, endianess);
			output.WriteValueF32(this.IntensityRand, endianess);
			output.WriteValueF32(this.RadiusRand, endianess);
			output.WriteValueF32(this.FlickerIntensityFreq, endianess);
			output.WriteValueF32(this.FlickerIntensityFreqRand, endianess);
			output.WriteValueF32(this.FlickerIntensityMag, endianess);
			output.WriteValueF32(this.FlickerIntensityMagRand, endianess);
			output.WriteValueF32(this.FlickerRadiusFreq, endianess);
			output.WriteValueF32(this.FlickerRadiusFreqRand, endianess);
			output.WriteValueF32(this.FlickerRadiusMag, endianess);
			output.WriteValueF32(this.FlickerRadiusMagRand, endianess);
			this.StartColour.Serialize(output, endianess);
			this.EndColour.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<SpaceType>(output, endianess, this.EmitSpace);
			this.Offset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<SpaceType>(output, endianess, this.AttachSpace);
			output.WriteValueU64(this.AttachJoint, endianess);
			output.WriteValueB32(this.Falloff, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.AbortWhenInterrupted = input.ReadValueB32(endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.ParentObjectName = input.ReadValueU64(endianess);
			this.Wait = input.ReadValueF32(endianess);
			this.FadeIn = input.ReadValueF32(endianess);
			this.Duration = input.ReadValueF32(endianess);
			this.FadeOut = input.ReadValueF32(endianess);
			this.WaitRand = input.ReadValueF32(endianess);
			this.FadeInRand = input.ReadValueF32(endianess);
			this.DurationRand = input.ReadValueF32(endianess);
			this.FadeOutRand = input.ReadValueF32(endianess);
			this.Intensity = input.ReadValueF32(endianess);
			this.Radius = input.ReadValueF32(endianess);
			this.IntensityRand = input.ReadValueF32(endianess);
			this.RadiusRand = input.ReadValueF32(endianess);
			this.FlickerIntensityFreq = input.ReadValueF32(endianess);
			this.FlickerIntensityFreqRand = input.ReadValueF32(endianess);
			this.FlickerIntensityMag = input.ReadValueF32(endianess);
			this.FlickerIntensityMagRand = input.ReadValueF32(endianess);
			this.FlickerRadiusFreq = input.ReadValueF32(endianess);
			this.FlickerRadiusFreqRand = input.ReadValueF32(endianess);
			this.FlickerRadiusMag = input.ReadValueF32(endianess);
			this.FlickerRadiusMagRand = input.ReadValueF32(endianess);
			this.StartColour = new Color(input, endianess);
			this.EndColour = new Color(input, endianess);
			this.EmitSpace = BaseProperty.DeserializePropertyEnum<SpaceType>(input, endianess);
			this.Offset = new Vector(input, endianess);
			this.AttachSpace = BaseProperty.DeserializePropertyEnum<SpaceType>(input, endianess);
			this.AttachJoint = input.ReadValueU64(endianess);
			this.Falloff = input.ReadValueB32(endianess);
		}
	}
}
