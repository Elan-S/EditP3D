using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.FX_BulletTracer)]
	public class BulletTracerTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float Wait { get; set; }
		public int HeadParameters { get; set; }
		public ulong HeadParametersShader { get; set; }
		public float HeadParametersGenerateTime { get; set; }
		public float HeadParametersFadeInTime { get; set; }
		public float HeadParametersFadeOutTime { get; set; }
		public float HeadParametersFadeInTrailTime { get; set; }
		public float HeadParametersFadeOutTrailTime { get; set; }
		public float HeadParametersShrinkInTime { get; set; }
		public float HeadParametersShrinkOutTime { get; set; }
		public float HeadParametersThickness { get; set; }
		public float HeadParametersTextureLength { get; set; }
		public float HeadParametersTextureScrollRate { get; set; }
		public Color HeadParametersStartColour { get; set; } = new Color();
		public Color HeadParametersEndColour { get; set; } = new Color();
		public int TailParameters { get; set; }
		public ulong TailParametersShader { get; set; }
		public float TailParametersGenerateTime { get; set; }
		public float TailParametersFadeInTime { get; set; }
		public float TailParametersFadeOutTime { get; set; }
		public float TailParametersFadeInTrailTime { get; set; }
		public float TailParametersFadeOutTrailTime { get; set; }
		public float TailParametersShrinkInTime { get; set; }
		public float TailParametersShrinkOutTime { get; set; }
		public float TailParametersThickness { get; set; }
		public float TailParametersTextureLength { get; set; }
		public float TailParametersTextureScrollRate { get; set; }
		public Color TailParametersStartColour { get; set; } = new Color();
		public Color TailParametersEndColour { get; set; } = new Color();
		public Vector Offset { get; set; } = new Vector();
		public ulong AttachJoint { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.Wait, endianess);
			output.WriteValueS32(this.HeadParameters, endianess);
			output.WriteValueU64(this.HeadParametersShader, endianess);
			output.WriteValueF32(this.HeadParametersGenerateTime, endianess);
			output.WriteValueF32(this.HeadParametersFadeInTime, endianess);
			output.WriteValueF32(this.HeadParametersFadeOutTime, endianess);
			output.WriteValueF32(this.HeadParametersFadeInTrailTime, endianess);
			output.WriteValueF32(this.HeadParametersFadeOutTrailTime, endianess);
			output.WriteValueF32(this.HeadParametersShrinkInTime, endianess);
			output.WriteValueF32(this.HeadParametersShrinkOutTime, endianess);
			output.WriteValueF32(this.HeadParametersThickness, endianess);
			output.WriteValueF32(this.HeadParametersTextureLength, endianess);
			output.WriteValueF32(this.HeadParametersTextureScrollRate, endianess);
			this.HeadParametersStartColour.Serialize(output, endianess);
			this.HeadParametersEndColour.Serialize(output, endianess);
			output.WriteValueS32(this.TailParameters, endianess);
			output.WriteValueU64(this.TailParametersShader, endianess);
			output.WriteValueF32(this.TailParametersGenerateTime, endianess);
			output.WriteValueF32(this.TailParametersFadeInTime, endianess);
			output.WriteValueF32(this.TailParametersFadeOutTime, endianess);
			output.WriteValueF32(this.TailParametersFadeInTrailTime, endianess);
			output.WriteValueF32(this.TailParametersFadeOutTrailTime, endianess);
			output.WriteValueF32(this.TailParametersShrinkInTime, endianess);
			output.WriteValueF32(this.TailParametersShrinkOutTime, endianess);
			output.WriteValueF32(this.TailParametersThickness, endianess);
			output.WriteValueF32(this.TailParametersTextureLength, endianess);
			output.WriteValueF32(this.TailParametersTextureScrollRate, endianess);
			this.TailParametersStartColour.Serialize(output, endianess);
			this.TailParametersEndColour.Serialize(output, endianess);
			this.Offset.Serialize(output, endianess);
			output.WriteValueU64(this.AttachJoint, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Wait = input.ReadValueF32(endianess);
			this.HeadParameters = input.ReadValueS32(endianess);
			this.HeadParametersShader = input.ReadValueU64(endianess);
			this.HeadParametersGenerateTime = input.ReadValueF32(endianess);
			this.HeadParametersFadeInTime = input.ReadValueF32(endianess);
			this.HeadParametersFadeOutTime = input.ReadValueF32(endianess);
			this.HeadParametersFadeInTrailTime = input.ReadValueF32(endianess);
			this.HeadParametersFadeOutTrailTime = input.ReadValueF32(endianess);
			this.HeadParametersShrinkInTime = input.ReadValueF32(endianess);
			this.HeadParametersShrinkOutTime = input.ReadValueF32(endianess);
			this.HeadParametersThickness = input.ReadValueF32(endianess);
			this.HeadParametersTextureLength = input.ReadValueF32(endianess);
			this.HeadParametersTextureScrollRate = input.ReadValueF32(endianess);
			this.HeadParametersStartColour.Deserialize(input, endianess);
			this.HeadParametersEndColour.Deserialize(input, endianess);
			this.TailParameters = input.ReadValueS32(endianess);
			this.TailParametersShader = input.ReadValueU64(endianess);
			this.TailParametersGenerateTime = input.ReadValueF32(endianess);
			this.TailParametersFadeInTime = input.ReadValueF32(endianess);
			this.TailParametersFadeOutTime = input.ReadValueF32(endianess);
			this.TailParametersFadeInTrailTime = input.ReadValueF32(endianess);
			this.TailParametersFadeOutTrailTime = input.ReadValueF32(endianess);
			this.TailParametersShrinkInTime = input.ReadValueF32(endianess);
			this.TailParametersShrinkOutTime = input.ReadValueF32(endianess);
			this.TailParametersThickness = input.ReadValueF32(endianess);
			this.TailParametersTextureLength = input.ReadValueF32(endianess);
			this.TailParametersTextureScrollRate = input.ReadValueF32(endianess);
			this.TailParametersStartColour.Deserialize(input, endianess);
			this.TailParametersEndColour.Deserialize(input, endianess);
			this.Offset.Deserialize(input, endianess);
			this.AttachJoint = input.ReadValueU64(endianess);
		}
	}
}
