using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownTrack(TrackHash.SpecialistDodge)]
	public class SpecialistDodgeTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float MoveBegin { get; set; }
		public float MoveEnd { get; set; }
		public float MoveDistance { get; set; }
		public int Priority { get; set; }
		public float BlendInTime { get; set; }
		public float BlendOutTime { get; set; }
		public float DistanceToAttractorWeight { get; set; }
		public float MaxDistanceToAttractor { get; set; }
		public PropertyTrackGroup LeftAnim { get; set; } = new PropertyTrackGroup(PropertyHash.LeftAnim);
		public PropertyTrackGroup RightAnim { get; set; } = new PropertyTrackGroup(PropertyHash.RightAnim);
		public PropertyTrackGroup BackAnim { get; set; } = new PropertyTrackGroup(PropertyHash.BackAnim);
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.MoveBegin, endianess);
			output.WriteValueF32(this.MoveEnd, endianess);
			output.WriteValueF32(this.MoveDistance, endianess);
			output.WriteValueS32(this.Priority, endianess);
			output.WriteValueF32(this.BlendInTime, endianess);
			output.WriteValueF32(this.BlendOutTime, endianess);
			output.WriteValueF32(this.DistanceToAttractorWeight, endianess);
			output.WriteValueF32(this.MaxDistanceToAttractor, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.MoveBegin = input.ReadValueF32(endianess);
			this.MoveEnd = input.ReadValueF32(endianess);
			this.MoveDistance = input.ReadValueF32(endianess);
			this.Priority = input.ReadValueS32(endianess);
			this.BlendInTime = input.ReadValueF32(endianess);
			this.BlendOutTime = input.ReadValueF32(endianess);
			this.DistanceToAttractorWeight = input.ReadValueF32(endianess);
			this.MaxDistanceToAttractor = input.ReadValueF32(endianess);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.LeftAnim);
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.RightAnim);
			BaseProperty.SerializeBaseProperty(PrototypeGame.P1, output, endianess, this.BackAnim);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			this.LeftAnim = BaseProperty.DeserializeTrackProperty(PrototypeGame.P1, input, endianess, PropertyHash.LeftAnim);
			this.RightAnim = BaseProperty.DeserializeTrackProperty(PrototypeGame.P1, input, endianess, PropertyHash.RightAnim);
			this.BackAnim = BaseProperty.DeserializeTrackProperty(PrototypeGame.P1, input, endianess, PropertyHash.BackAnim);
		}
	}
}
