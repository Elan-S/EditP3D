using System;

namespace Nixson.Prototype.Fight
{
	[Flags]
	public enum TargetFilterType : ulong
	{
		LockOn = 1UL,
		AttackMelee = 2UL,
		AttackRanged = 4UL,
		Grab = 8UL,
		Feet = 16UL,
		AlertObject = 32UL
	}
}
