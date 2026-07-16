using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.HasPhysics)]
	public class HasPhysicsCondition : P1Condition
	{
		public bool ValidPhysicsObject { get; set; }
		public HasPhysicsCondition.InContextType InContext { get; set; }
		public HasPhysicsCondition.EnabledCollisionObjectsType EnabledCollisionObjects { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.ValidPhysicsObject, endianess);
			BaseProperty.SerializePropertyEnum<HasPhysicsCondition.InContextType>(output, endianess, this.InContext);
			BaseProperty.SerializePropertyEnum<HasPhysicsCondition.EnabledCollisionObjectsType>(output, endianess, this.EnabledCollisionObjects);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.ValidPhysicsObject = input.ReadValueB32(endianess);
			this.InContext = BaseProperty.DeserializePropertyEnum<HasPhysicsCondition.InContextType>(input, endianess);
			this.EnabledCollisionObjects = BaseProperty.DeserializePropertyEnum<HasPhysicsCondition.EnabledCollisionObjectsType>(input, endianess);
		}
		public enum InContextType : ulong
		{
			Yes = 382980737677UL,
			No = 5116765UL,
			DontCare = 8061513285755875840UL
		}
		public enum EnabledCollisionObjectsType : ulong
		{
			All = 279702787393UL,
			None = 22018610510307286UL,
			AtLeastOne = 16128247779282615688UL,
			DontCare = 8061513285755875840UL
		}
	}
}
