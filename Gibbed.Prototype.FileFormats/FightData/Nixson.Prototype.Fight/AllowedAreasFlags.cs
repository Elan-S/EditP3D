using System;

namespace Nixson.Prototype.Fight
{
	[Flags]
	public enum AllowedAreasFlags : ulong
	{
		Unknown = 1UL,
		Intersection = 2UL,
		Road = 4UL,
		Crosswalk = 8UL,
		Sidewalk = 16UL,
		CrosswalkCorner = 32UL,
		Allyway = 64UL,
		Rooftop = 128UL
	}
}
