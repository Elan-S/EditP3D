using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownCondition(13935992383628624466UL)]
	public class PedestrianAlertStateCondition : P1Condition
	{
		public bool Match { get; set; }
		public PedestrianAlertStateCondition.PedReactState State { get; set; }
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueB32(this.Match, endianess);
			BaseProperty.SerializePropertyBitfield<PedestrianAlertStateCondition.PedReactState>(output, endianess, this.State);
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			this.Match = input.ReadValueB32(endianess);
			this.State = BaseProperty.DeserializePropertyBitfield<PedestrianAlertStateCondition.PedReactState>(input, endianess);
		}
		[Flags]
		public enum PedReactState : ulong
		{
			None = 1UL,
			Explosion = 2UL,
			Throw = 4UL,
			Shapeshift = 8UL,
			Gunfire = 16UL,
			Grab = 32UL,
			Hit = 64UL,
			Shove = 128UL,
			ExtremeLocomotion = 256UL,
			Landing = 512UL,
			Consume = 1024UL,
			Death = 2048UL,
			BulletHit = 4096UL,
			ScatterPeds = 8192UL,
			MissileFire = 16384UL,
			PanickingPedestrian = 32768UL,
			DeadBody = 65536UL,
			GrabScream = 131072UL,
			FlyingObject = 262144UL,
			ThrowLand = 524288UL,
			EnterVehicle = 1048576UL,
			GenerateDiversion = 2097152UL,
			BurningVehicle = 4194304UL,
			CarAccident = 8388608UL
		}
	}
}
