using System;

namespace Nixson.Prototype.Fight
{
	[Flags]
	public enum AnimationAllowedTransitionsType : ulong
	{
		North = 1UL,
		East = 2UL,
		South = 4UL,
		West = 8UL
	}
}
