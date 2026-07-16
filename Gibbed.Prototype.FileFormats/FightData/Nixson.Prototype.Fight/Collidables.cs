using System;

namespace Nixson.Prototype.Fight
{
	[Flags]
	public enum Collidables : ulong
	{
		StaticObject = 1UL,
		SimulatedObject = 2UL,
		SupportingSurface = 4UL,
		Attachable = 8UL,
		Health = 16UL,
		HitReaction = 32UL,
		Prop = 64UL,
		CharacterSolver = 128UL,
		CauseDamage = 256UL,
		CauseHitReaction = 512UL,
		TriggerVolume = 1024UL,
		Grabbable = 2048UL,
		NavigationSimulatedObstacle = 4096UL,
		AmbientLookaheadVolume = 8192UL,
		PedBehaviour = 16384UL,
		VehicleBehaviour = 32768UL,
		Building = 65536UL,
		Dead = 131072UL,
		Tank = 262144UL,
		Walker = 524288UL,
		Flyer = 1048576UL,
		Hunter = 2097152UL,
		World = 4194304UL,
		CharacterShield = 8388608UL,
		District = 16777216UL,
		Zone = 33554432UL,
		Player = 67108864UL,
		CameraPop = 134217728UL,
		Shoveable = 268435456UL,
		Pole = 536870912UL,
		NISExclusionVolume = 1073741824UL,
		NavigationStaticObstacle = 2147483648UL
	}
}
