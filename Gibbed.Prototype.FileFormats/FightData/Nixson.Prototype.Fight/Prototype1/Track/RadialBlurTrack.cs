using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(12442154806601551562UL)]
	public class RadialBlurTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Wait { get; set; }
		public float FadeIn { get; set; }
		public float Duration { get; set; }
		public float FadeOut { get; set; }
		public float WaitRand { get; set; }
		public float FadeInRand { get; set; }
		public float DurationRand { get; set; }
		public float FadeOutRand { get; set; }
		public float IntensityStart { get; set; }
		public float IntensityEnd { get; set; }
		public float SizeStart { get; set; }
		public float SizeEnd { get; set; }
		public float FalloffStart { get; set; }
		public float FalloffEnd { get; set; }
		public float FlickerIntensityFreq { get; set; }
		public float FlickerIntensityFreqRand { get; set; }
		public float FlickerIntensityMag { get; set; }
		public float FlickerIntensityMagRand { get; set; }
		public float FlickerSizeFreq { get; set; }
		public float FlickerSizeFreqRand { get; set; }
		public float FlickerSizeMag { get; set; }
		public float FlickerSizeMagRand { get; set; }
		public float FlickerFalloffFreq { get; set; }
		public float FlickerFalloffFreqRand { get; set; }
		public float FlickerFalloffMag { get; set; }
		public float FlickerFalloffMagRand { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public AttachSpaceType AttachSpace { get; set; }
		public ulong AttachJoint { get; set; }
		public string Patch { get; set; }
		public bool InGameCameraOnly { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Wait, endianess);
			output.WriteValueF32(this.FadeIn, endianess);
			output.WriteValueF32(this.Duration, endianess);
			output.WriteValueF32(this.FadeOut, endianess);
			output.WriteValueF32(this.WaitRand, endianess);
			output.WriteValueF32(this.FadeInRand, endianess);
			output.WriteValueF32(this.DurationRand, endianess);
			output.WriteValueF32(this.FadeOutRand, endianess);
			output.WriteValueF32(this.IntensityStart, endianess);
			output.WriteValueF32(this.IntensityEnd, endianess);
			output.WriteValueF32(this.SizeStart, endianess);
			output.WriteValueF32(this.SizeEnd, endianess);
			output.WriteValueF32(this.FalloffStart, endianess);
			output.WriteValueF32(this.FalloffEnd, endianess);
			output.WriteValueF32(this.FlickerIntensityFreq, endianess);
			output.WriteValueF32(this.FlickerIntensityFreqRand, endianess);
			output.WriteValueF32(this.FlickerIntensityMag, endianess);
			output.WriteValueF32(this.FlickerIntensityMagRand, endianess);
			output.WriteValueF32(this.FlickerSizeFreq, endianess);
			output.WriteValueF32(this.FlickerSizeFreqRand, endianess);
			output.WriteValueF32(this.FlickerSizeMag, endianess);
			output.WriteValueF32(this.FlickerSizeMagRand, endianess);
			output.WriteValueF32(this.FlickerFalloffFreq, endianess);
			output.WriteValueF32(this.FlickerFalloffFreqRand, endianess);
			output.WriteValueF32(this.FlickerFalloffMag, endianess);
			output.WriteValueF32(this.FlickerFalloffMagRand, endianess);
			this.Offset.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<AttachSpaceType>(output, endianess, this.AttachSpace);
			output.WriteValueU64(this.AttachJoint, endianess);
			output.WriteStringAlignedU32(this.Patch, endianess);
			output.WriteValueB32(this.InGameCameraOnly, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Wait = input.ReadValueF32(endianess);
			this.FadeIn = input.ReadValueF32(endianess);
			this.Duration = input.ReadValueF32(endianess);
			this.FadeOut = input.ReadValueF32(endianess);
			this.WaitRand = input.ReadValueF32(endianess);
			this.FadeInRand = input.ReadValueF32(endianess);
			this.DurationRand = input.ReadValueF32(endianess);
			this.FadeOutRand = input.ReadValueF32(endianess);
			this.IntensityStart = input.ReadValueF32(endianess);
			this.IntensityEnd = input.ReadValueF32(endianess);
			this.SizeStart = input.ReadValueF32(endianess);
			this.SizeEnd = input.ReadValueF32(endianess);
			this.FalloffStart = input.ReadValueF32(endianess);
			this.FalloffEnd = input.ReadValueF32(endianess);
			this.FlickerIntensityFreq = input.ReadValueF32(endianess);
			this.FlickerIntensityFreqRand = input.ReadValueF32(endianess);
			this.FlickerIntensityMag = input.ReadValueF32(endianess);
			this.FlickerIntensityMagRand = input.ReadValueF32(endianess);
			this.FlickerSizeFreq = input.ReadValueF32(endianess);
			this.FlickerSizeFreqRand = input.ReadValueF32(endianess);
			this.FlickerSizeMag = input.ReadValueF32(endianess);
			this.FlickerSizeMagRand = input.ReadValueF32(endianess);
			this.FlickerFalloffFreq = input.ReadValueF32(endianess);
			this.FlickerFalloffFreqRand = input.ReadValueF32(endianess);
			this.FlickerFalloffMag = input.ReadValueF32(endianess);
			this.FlickerFalloffMagRand = input.ReadValueF32(endianess);
			this.Offset = new Vector(input, endianess);
			this.AttachSpace = BaseProperty.DeserializePropertyEnum<AttachSpaceType>(input, endianess);
			this.AttachJoint = input.ReadValueU64(endianess);
			this.Patch = input.ReadStringAlignedU32(endianess);
			this.InGameCameraOnly = input.ReadValueB32(endianess);
		}
	}
}
