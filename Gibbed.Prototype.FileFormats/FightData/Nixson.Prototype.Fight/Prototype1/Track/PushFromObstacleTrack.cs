using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.PushFromObstacle)]
	public class PushFromObstacleTrack : P1Track
	{
		public float BeginTime { get; set; }
		public float EndTime { get; set; }
		public ulong JointName { get; set; }
		public Vector Offset { get; set; } = new Vector();
		public Vector RayVector { get; set; } = new Vector();
		public Collidables CollideWith { get; set; }
		public float Velocity { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.BeginTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			output.WriteValueU64(this.JointName, endianess);
			this.Offset.Serialize(output, endianess);
			this.RayVector.Serialize(output, endianess);
			BaseProperty.SerializePropertyBitfield<Collidables>(output, endianess, this.CollideWith);
			output.WriteValueF32(this.Velocity, endianess);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.BeginTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
			this.JointName = input.ReadValueU64(endianess);
			this.Offset = new Vector(input, endianess);
			this.RayVector = new Vector(input, endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<Collidables>(input, endianess);
			this.Velocity = input.ReadValueF32(endianess);
		}
	}
}
