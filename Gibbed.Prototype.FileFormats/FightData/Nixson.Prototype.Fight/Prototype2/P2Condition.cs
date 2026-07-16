using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight.Prototype2
{
	public class P2Condition : BaseCondition
	{
		public static void SerializeBaseCondition(Stream output, Endian endianess, BaseCondition condition)
		{
			Stream stream = new MemoryStream();
			condition.Serialize(stream, endianess);
			output.WriteValueU64(condition.TypeHash, endianess);
			output.WriteValueU32((uint)stream.Length, endianess);
			stream.Seek(0L, SeekOrigin.Begin);
			output.WriteFromStream(stream, stream.Length);
			condition.SerializeProperties(PrototypeGame.P2, output, endianess);
			output.WriteValueU64(0UL);
		}
		public static void SerializeBaseConditions(Stream output, Endian endianess, List<BaseCondition> conditions)
		{
			foreach (BaseCondition condition in conditions)
			{
				P2Condition.SerializeBaseCondition(output, endianess, condition);
			}
		}
		public static BaseCondition DeserializeBaseCondition(Stream input, Endian endianess, ulong hash)
		{
			BaseCondition baseCondition = Factory<BaseCondition, KnownConditionAttribute>.Build(PrototypeGame.P2, hash);
			if (baseCondition == null)
			{
				throw new NotImplementedException("Unknown condition");
			}
			uint num = input.ReadValueU32(endianess);
			long position = input.Position;
			baseCondition.Deserialize(input, endianess);
			if (input.Position != position + (long)((ulong)num))
			{
				throw new FormatException("Invalid condition length");
			}
			baseCondition.DeserializeProperties(PrototypeGame.P2, input, endianess);
			input.ReadValueU64();
			return baseCondition;
		}
		public static List<BaseCondition> DeserializeBaseConditions(Stream input, Endian endianess)
		{
			List<BaseCondition> list = new List<BaseCondition>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				list.Add(P2Condition.DeserializeBaseCondition(input, endianess, num));
			}
			input.Position -= 8L;
			return list;
		}
	}
}
