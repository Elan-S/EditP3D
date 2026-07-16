using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(15365642249572122508UL)]
	public class WhipFistExtendTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float VelocityMin { get; set; }
		public float VelocityMax { get; set; }
		public float DistanceMin { get; set; }
		public float DistanceMax { get; set; }
		public float TrackingMin { get; set; }
		public float TrackingMax { get; set; }
		public float DamageMin { get; set; }
		public float DamageMax { get; set; }
		public float WaveSpeed { get; set; }
		public float WaveLength { get; set; }
		public float WaveAmplitude { get; set; }
		public float WaveAmplitudeRampUp { get; set; }
		public float WaveAmplitudeRampDown { get; set; }
		public float CorkscrewAngle { get; set; }
		public float CorkscrewRotationSpeed { get; set; }
		public float CorkscrewTravelingSpeed { get; set; }
		public WhipFistExtendTrack.WhipfistExtendTargetType Target { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.VelocityMin, endianess);
			output.WriteValueF32(this.VelocityMax, endianess);
			output.WriteValueF32(this.DistanceMin, endianess);
			output.WriteValueF32(this.DistanceMax, endianess);
			output.WriteValueF32(this.TrackingMin, endianess);
			output.WriteValueF32(this.TrackingMax, endianess);
			output.WriteValueF32(this.DamageMin, endianess);
			output.WriteValueF32(this.DamageMax, endianess);
			output.WriteValueF32(this.WaveSpeed, endianess);
			output.WriteValueF32(this.WaveLength, endianess);
			output.WriteValueF32(this.WaveAmplitude, endianess);
			output.WriteValueF32(this.WaveAmplitudeRampUp, endianess);
			output.WriteValueF32(this.WaveAmplitudeRampDown, endianess);
			output.WriteValueF32(this.CorkscrewAngle, endianess);
			output.WriteValueF32(this.CorkscrewRotationSpeed, endianess);
			output.WriteValueF32(this.CorkscrewTravelingSpeed, endianess);
			BaseProperty.SerializePropertyEnum<WhipFistExtendTrack.WhipfistExtendTargetType>(output, endianess, this.Target);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.VelocityMin = input.ReadValueF32(endianess);
			this.VelocityMax = input.ReadValueF32(endianess);
			this.DistanceMin = input.ReadValueF32(endianess);
			this.DistanceMax = input.ReadValueF32(endianess);
			this.TrackingMin = input.ReadValueF32(endianess);
			this.TrackingMax = input.ReadValueF32(endianess);
			this.DamageMin = input.ReadValueF32(endianess);
			this.DamageMax = input.ReadValueF32(endianess);
			this.WaveSpeed = input.ReadValueF32(endianess);
			this.WaveLength = input.ReadValueF32(endianess);
			this.WaveAmplitude = input.ReadValueF32(endianess);
			this.WaveAmplitudeRampUp = input.ReadValueF32(endianess);
			this.WaveAmplitudeRampDown = input.ReadValueF32(endianess);
			this.CorkscrewAngle = input.ReadValueF32(endianess);
			this.CorkscrewRotationSpeed = input.ReadValueF32(endianess);
			this.CorkscrewTravelingSpeed = input.ReadValueF32(endianess);
			this.Target = BaseProperty.DeserializePropertyEnum<WhipFistExtendTrack.WhipfistExtendTargetType>(input, endianess);
		}
		public enum WhipfistExtendTargetType : ulong
		{
			Target = 856854631462190855UL,
			GrabTarget = 1754404701201221985UL
		}
	}
}
