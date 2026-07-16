using System;

namespace Nixson.Prototype.Fight
{
	[Flags]
	public enum ColliderType : ulong
	{
		Reserved = 1UL,
		StaticObject = 2UL,
		StaticObjectHighVelocity = 4UL,
		SimulatedObject = 8UL,
		SupportingSurface = 16UL,
		Attachable = 32UL,
		Health = 64UL,
		HitReaction = 128UL,
		Prop = 256UL,
		CharacterSolver = 512UL,
		CauseDamage = 1024UL,
		CauseHitReaction = 2048UL,
		TriggerVolume = 4096UL,
		Camera = 8192UL,
		CameraPop = 16384UL,
		CameraFade = 32768UL,
		Grabbable = 65536UL,
		NavigationSimulatedObstacle = 131072UL,
		AmbientLookaheadVolume = 262144UL,
		PedBehaviour = 524288UL,
		VehicleBehaviour = 1048576UL,
		Dead = 2097152UL,
		Tank = 4194304UL,
		Walker = 8388608UL,
		Flyer = 16777216UL,
		Hunter = 33554432UL,
		World = 67108864UL,
		CharacterShield = 134217728UL,
		Water = 268435456UL,
		Zone = 536870912UL,
		Player = 1073741824UL,
		Shoveable = 2147483648UL,
		Pole = 4294967296UL,
		NISExclusionVolume = 8589934592UL,
		NavigationStaticObstacle = 17179869184UL
	}
}
