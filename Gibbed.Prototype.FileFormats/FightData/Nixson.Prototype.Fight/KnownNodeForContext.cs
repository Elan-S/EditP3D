using System;

namespace Nixson.Prototype.Fight
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class KnownNodeForContext : Attribute
	{
		public KnownNodeForContext(ulong contextHash)
		{
			this.ContextHash = contextHash;
		}
		public KnownNodeForContext(ContextHash contextHash)
		{
			this.ContextHash = (ulong)contextHash;
		}
		public ulong ContextHash;
	}
}
