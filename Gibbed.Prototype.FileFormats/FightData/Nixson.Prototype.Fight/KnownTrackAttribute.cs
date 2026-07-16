using System;

namespace Nixson.Prototype.Fight
{
	[AttributeUsage(AttributeTargets.Class)]
	public class KnownTrackAttribute : KnownHashAttribute
	{
		public KnownTrackAttribute(ulong hash) : base(hash)
		{
		}
		public KnownTrackAttribute(TrackHash hashEnum) : base((ulong)hashEnum)
		{
		}
	}
}
