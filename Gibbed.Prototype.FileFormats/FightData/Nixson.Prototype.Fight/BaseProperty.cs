using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Property;

namespace Nixson.Prototype.Fight
{
	public abstract class BaseProperty : FightNode
	{
		public BaseProperty()
		{
		}
		public BaseProperty(PropertyHash hash)
		{
			base.TypeHash = (ulong)hash;
		}
		public static void SerializePropertyEnum<T>(Stream stream, Endian endianess, T value) where T : Enum
		{
			if (!Enum.IsDefined(typeof(T), value))
			{
				throw new NotImplementedException("Enum not defined");
			}
			object obj = Enum.ToObject(typeof(T), value);
			stream.WriteValueU64((ulong)obj, endianess);
		}
		public static T DeserializePropertyEnum<T>(Stream stream, Endian endianess) where T : Enum
		{
			ulong num = stream.ReadValueU64(endianess);
			if (!Enum.IsDefined(typeof(T), num))
			{
				throw new NotImplementedException("Enum not defined");
			}
			return (T)((object)Enum.ToObject(typeof(T), num));
		}
		public static void SerializePropertyBitfield<T>(Stream stream, Endian endianess, T value) where T : Enum
		{
			ulong value2 = (ulong)Enum.ToObject(typeof(T), value);
			stream.WriteValueU32(Convert.ToUInt32(value2), endianess);
		}
		public static T DeserializePropertyBitfield<T>(Stream stream, Endian endianess) where T : Enum
		{
			ulong value = Convert.ToUInt64(stream.ReadValueU32(endianess));
			return (T)((object)Enum.ToObject(typeof(T), value));
		}
		public static PropertyConditionGroup DeserializeConditionProperty(PrototypeGame game, Stream input, Endian endianess, PropertyHash hash)
		{
			return (PropertyConditionGroup)BaseProperty.DeserializeBaseProperty(game, input, endianess, hash, false);
		}
		public static List<BaseProperty> DeserializeConditionProperties(PrototypeGame game, Stream input, Endian endianess, PropertyHash target)
		{
			return BaseProperty.DeserializeBaseProperties(game, input, endianess, target, false);
		}
		public static PropertyTrackGroup DeserializeTrackProperty(PrototypeGame game, Stream input, Endian endianess, PropertyHash hash)
		{
			return (PropertyTrackGroup)BaseProperty.DeserializeBaseProperty(game, input, endianess, hash, true);
		}
		public static List<BaseProperty> DeserializeTrackProperties(PrototypeGame game, Stream input, Endian endianess, PropertyHash target)
		{
			return BaseProperty.DeserializeBaseProperties(game, input, endianess, target, true);
		}
		public static void SerializeBaseProperty(PrototypeGame game, Stream output, Endian endianess, BaseProperty property)
		{
			if (game == PrototypeGame.P1)
			{
				BaseProperty.P1_SerializeBaseProperty(output, endianess, property);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			BaseProperty.P2_SerializeBaseProperty(output, endianess, property);
		}
		public static void SerializeBaseProperties(PrototypeGame game, Stream output, Endian endianess, List<BaseProperty> properties)
		{
			if (game == PrototypeGame.P1)
			{
				BaseProperty.P1_SerializeBaseProperties(output, endianess, properties);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			BaseProperty.P2_SerializeBaseProperties(output, endianess, properties);
		}
		private static BaseProperty DeserializeBaseProperty(PrototypeGame game, Stream input, Endian endianess, PropertyHash hash, bool isTrack)
		{
			if (game == PrototypeGame.P1)
			{
				return BaseProperty.P1_DeserializeBaseProperty(input, endianess, hash, isTrack);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return BaseProperty.P2_DeserializeBaseProperty(input, endianess, hash, isTrack);
		}
		private static List<BaseProperty> DeserializeBaseProperties(PrototypeGame game, Stream input, Endian endianess, PropertyHash target, bool isTrack)
		{
			if (game == PrototypeGame.P1)
			{
				return BaseProperty.P1_DeserializeBaseProperties(input, endianess, target, isTrack);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return BaseProperty.P2_DeserializeBaseProperties(input, endianess, target, isTrack);
		}
		private static void P1_SerializeBaseProperty(Stream output, Endian endianess, BaseProperty property)
		{
			output.WriteValueU64(property.TypeHash, endianess);
			Stream stream = new MemoryStream();
			property.Serialize(stream, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			property.SerializeProperties(PrototypeGame.P1, output, endianess);
			output.WriteValueU64(0UL);
		}
		private static void P1_SerializeBaseProperties(Stream output, Endian endianess, List<BaseProperty> properties)
		{
			foreach (BaseProperty property in properties)
			{
				BaseProperty.P1_SerializeBaseProperty(output, endianess, property);
			}
		}
		private static BaseProperty P1_DeserializeBaseProperty(Stream input, Endian endianess, PropertyHash hash, bool isTrack)
		{
			if (hash != (PropertyHash)input.ReadValueU64(endianess))
			{
				throw new Exception("Read wrong property hash");
			}
			BaseProperty baseProperty;
			if (isTrack)
			{
				baseProperty = new PropertyTrackGroup(hash);
			}
			else
			{
				baseProperty = new PropertyConditionGroup(hash);
			}
			if (baseProperty == null)
			{
				throw new NotImplementedException("Unknown property");
			}
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			baseProperty.Deserialize(input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid property length");
			}
			baseProperty.DeserializeProperties(PrototypeGame.P1, input, endianess);
			return baseProperty;
		}
		private static List<BaseProperty> P1_DeserializeBaseProperties(Stream input, Endian endianess, PropertyHash target, bool isTrack)
		{
			List<BaseProperty> list = new List<BaseProperty>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (target != (PropertyHash)0UL && num != (ulong)target)
				{
					break;
				}
				list.Add(BaseProperty.P1_DeserializeBaseProperty(input, endianess, target, isTrack));
			}
			input.Position -= 8L;
			return list;
		}
		private static void P2_SerializeBaseProperty(Stream output, Endian endianess, BaseProperty property)
		{
			output.WriteValueU64(property.TypeHash, endianess);
			Stream stream = new MemoryStream();
			property.Serialize(stream, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			property.SerializeProperties(PrototypeGame.P2, output, endianess);
			output.WriteValueU64(0UL);
		}
		private static void P2_SerializeBaseProperties(Stream output, Endian endianess, List<BaseProperty> properties)
		{
			foreach (BaseProperty property in properties)
			{
				BaseProperty.P2_SerializeBaseProperty(output, endianess, property);
			}
		}
		private static BaseProperty P2_DeserializeBaseProperty(Stream input, Endian endianess, PropertyHash hash, bool isTrack)
		{
			if (hash != (PropertyHash)input.ReadValueU64(endianess))
			{
				throw new Exception("Read wrong property hash");
			}
			BaseProperty baseProperty;
			if (isTrack)
			{
				baseProperty = new PropertyTrackGroup(hash);
			}
			else
			{
				baseProperty = new PropertyConditionGroup(hash);
			}
			if (baseProperty == null)
			{
				throw new NotImplementedException("Unknown property");
			}
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			input.ReadValueS32();
			baseProperty.Deserialize(input, endianess);
			baseProperty.DeserializeProperties(PrototypeGame.P2, input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid property length");
			}
			input.ReadValueU64();
			return baseProperty;
		}
		private static List<BaseProperty> P2_DeserializeBaseProperties(Stream input, Endian endianess, PropertyHash target, bool isTrack)
		{
			List<BaseProperty> list = new List<BaseProperty>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (target != (PropertyHash)0UL && num != (ulong)target)
				{
					break;
				}
				list.Add(BaseProperty.P2_DeserializeBaseProperty(input, endianess, target, isTrack));
			}
			input.Position -= 8L;
			return list;
		}
	}
}
