using System;

namespace Nixson.Prototype.Fight
{
	public abstract class KnownHashAttribute : Attribute
	{
		public KnownHashAttribute(ulong hash)
		{
			this.Hash = hash;
		}
		public ulong Hash;
	}
}
