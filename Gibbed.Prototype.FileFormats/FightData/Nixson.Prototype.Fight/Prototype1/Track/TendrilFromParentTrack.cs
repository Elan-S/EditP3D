using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(9262301513045246984UL)]
	public class TendrilFromParentTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeReverse { get; set; }
		public float TimeEnd { get; set; }
		public ulong Drawable { get; set; }
		public float Scale { get; set; }
		public bool KeepGoing { get; set; }
		public float HitRadius { get; set; }
		public float TrackingVelocity { get; set; }
		public float TrackingDeceleration { get; set; }
		public bool SwapDirection { get; set; }
		public bool StaticEndPoint { get; set; }
		public ulong StartJoint { get; set; }
		public Vector StartJointOffset { get; set; } = new Vector();
		public ulong GrabSlotJoint { get; set; }
		public Vector GrabSlotJointOffset { get; set; } = new Vector();
		public float ForwardVelocity { get; set; }
		public float ReverseVelocity { get; set; }
		public float CircleRadius { get; set; }
		public float ZAxisRotation { get; set; }
		public float XAxisRotation { get; set; }
		public int SegmentCount { get; set; }
		public float WaveSpeed { get; set; }
		public float WaveLength { get; set; }
		public float WaveAmplitude { get; set; }
		public float WaveAmplitudeRampUp { get; set; }
		public float WaveAmplitudeRampDown { get; set; }
		public float CorkscrewAngle { get; set; }
		public float CorkscrewRotationSpeed { get; set; }
		public float CorkscrewTravelingSpeed { get; set; }
		public float DevastatorDamageMultiplier { get; set; }
		public float TimeBetweenHits { get; set; }
		public PropertyConditionGroup ReverseConditions { get; set; } = new PropertyConditionGroup(PropertyHash.ReverseConditions);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeReverse, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueU64(this.Drawable, endianess);
			output.WriteValueF32(this.Scale, endianess);
			output.WriteValueB32(this.KeepGoing, endianess);
			output.WriteValueF32(this.HitRadius, endianess);
			output.WriteValueF32(this.TrackingVelocity, endianess);
			output.WriteValueF32(this.TrackingDeceleration, endianess);
			output.WriteValueB32(this.SwapDirection, endianess);
			output.WriteValueB32(this.StaticEndPoint, endianess);
			output.WriteValueU64(this.StartJoint, endianess);
			this.StartJointOffset.Serialize(output, endianess);
			output.WriteValueU64(this.GrabSlotJoint, endianess);
			this.GrabSlotJointOffset.Serialize(output, endianess);
			output.WriteValueF32(this.ForwardVelocity, endianess);
			output.WriteValueF32(this.ReverseVelocity, endianess);
			output.WriteValueF32(this.CircleRadius, endianess);
			output.WriteValueF32(this.ZAxisRotation, endianess);
			output.WriteValueF32(this.XAxisRotation, endianess);
			output.WriteValueS32(this.SegmentCount, endianess);
			output.WriteValueF32(this.WaveSpeed, endianess);
			output.WriteValueF32(this.WaveLength, endianess);
			output.WriteValueF32(this.WaveAmplitude, endianess);
			output.WriteValueF32(this.WaveAmplitudeRampUp, endianess);
			output.WriteValueF32(this.WaveAmplitudeRampDown, endianess);
			output.WriteValueF32(this.CorkscrewAngle, endianess);
			output.WriteValueF32(this.CorkscrewRotationSpeed, endianess);
			output.WriteValueF32(this.CorkscrewTravelingSpeed, endianess);
			output.WriteValueF32(this.DevastatorDamageMultiplier, endianess);
			output.WriteValueF32(this.TimeBetweenHits, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeReverse = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.Drawable = input.ReadValueU64(endianess);
			this.Scale = input.ReadValueF32(endianess);
			this.KeepGoing = input.ReadValueB32(endianess);
			this.HitRadius = input.ReadValueF32(endianess);
			this.TrackingVelocity = input.ReadValueF32(endianess);
			this.TrackingDeceleration = input.ReadValueF32(endianess);
			this.SwapDirection = input.ReadValueB32(endianess);
			this.StaticEndPoint = input.ReadValueB32(endianess);
			this.StartJoint = input.ReadValueU64(endianess);
			this.StartJointOffset = new Vector(input, endianess);
			this.GrabSlotJoint = input.ReadValueU64(endianess);
			this.GrabSlotJointOffset = new Vector(input, endianess);
			this.ForwardVelocity = input.ReadValueF32(endianess);
			this.ReverseVelocity = input.ReadValueF32(endianess);
			this.CircleRadius = input.ReadValueF32(endianess);
			this.ZAxisRotation = input.ReadValueF32(endianess);
			this.XAxisRotation = input.ReadValueF32(endianess);
			this.SegmentCount = input.ReadValueS32(endianess);
			this.WaveSpeed = input.ReadValueF32(endianess);
			this.WaveLength = input.ReadValueF32(endianess);
			this.WaveAmplitude = input.ReadValueF32(endianess);
			this.WaveAmplitudeRampUp = input.ReadValueF32(endianess);
			this.WaveAmplitudeRampDown = input.ReadValueF32(endianess);
			this.CorkscrewAngle = input.ReadValueF32(endianess);
			this.CorkscrewRotationSpeed = input.ReadValueF32(endianess);
			this.CorkscrewTravelingSpeed = input.ReadValueF32(endianess);
			this.DevastatorDamageMultiplier = input.ReadValueF32(endianess);
			this.TimeBetweenHits = input.ReadValueF32(endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.ReverseConditions);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.ReverseConditions = BaseProperty.DeserializeConditionProperty(PrototypeGame.P1, input, endianess, PropertyHash.ReverseConditions);
		}
	}
}
