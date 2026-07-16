using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.GrabSlotDetachThrow)]
	public class GrabSlotDetachThrowTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public float TimeEnd { get; set; }
		public float LatestDetachTime { get; set; }
		public ulong GrabSlot { get; set; }
		public VelocityReference LinearVelocityReference { get; set; }
		public Vector AddedLinearVelocity { get; set; } = new Vector();
		public VelocityReference AngularVelocityReference { get; set; }
		public Vector AddedAngularVelocity { get; set; } = new Vector();
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			output.WriteValueF32(this.TimeEnd, endianess);
			output.WriteValueF32(this.LatestDetachTime, endianess);
			output.WriteValueU64(this.GrabSlot, endianess);
			BaseProperty.SerializePropertyEnum<VelocityReference>(output, endianess, this.LinearVelocityReference);
			this.AddedLinearVelocity.Serialize(output, endianess);
			BaseProperty.SerializePropertyEnum<VelocityReference>(output, endianess, this.AngularVelocityReference);
			this.AddedAngularVelocity.Serialize(output, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.TimeEnd = input.ReadValueF32(endianess);
			this.LatestDetachTime = input.ReadValueF32(endianess);
			this.GrabSlot = input.ReadValueU64(endianess);
			this.LinearVelocityReference = BaseProperty.DeserializePropertyEnum<VelocityReference>(input, endianess);
			this.AddedLinearVelocity = new Vector(input, endianess);
			this.AngularVelocityReference = BaseProperty.DeserializePropertyEnum<VelocityReference>(input, endianess);
			this.AddedAngularVelocity = new Vector(input, endianess);
		}
	}
}
