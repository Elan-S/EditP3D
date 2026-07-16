using System;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight
{
	public abstract class FightNode : IClonable
	{
		public ulong TypeHash { get; set; }
		public virtual void Serialize(Stream output, Endian endianess)
		{
		}
		public virtual void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
		}
		public virtual void Deserialize(Stream input, Endian endianess)
		{
		}
		public virtual void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
		}
		public abstract object Clone(PrototypeGame game);
	}
}
