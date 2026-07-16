using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownTrack(TrackHash.PhysicsSetIntersectionProperties)]
	public class PhysicsSetIntersectionPropertiesTrack : P1Track
	{
		public float TimeBegin { get; set; }
		public ColliderType Collider { get; set; }
		public Collidables CollideWith { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.TimeBegin, endianess);
			BaseProperty.SerializePropertyBitfield<ColliderType>(output, endianess, this.Collider);
			BaseProperty.SerializePropertyBitfield<Collidables>(output, endianess, this.CollideWith);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.TimeBegin = input.ReadValueF32(endianess);
			this.Collider = BaseProperty.DeserializePropertyBitfield<ColliderType>(input, endianess);
			this.CollideWith = BaseProperty.DeserializePropertyBitfield<Collidables>(input, endianess);
		}
	}
}
