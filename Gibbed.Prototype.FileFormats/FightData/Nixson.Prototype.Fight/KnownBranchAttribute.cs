using System;

namespace Nixson.Prototype.Fight
{
	[AttributeUsage(AttributeTargets.Class)]
	public class KnownBranchAttribute : KnownHashAttribute
	{
		public KnownBranchAttribute(ulong hash) : base(hash)
		{
		}
		public KnownBranchAttribute(BranchHash hashEnum) : base((ulong)hashEnum)
		{
		}
	}
}
