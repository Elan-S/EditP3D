using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;
using Nixson.Prototype.Fight.Prototype1;
using Nixson.Prototype.Fight.Prototype2;

namespace Nixson.Prototype.Fight
{
	public abstract class BaseCondition : FightNode
	{
		public BaseCondition()
		{
			KnownConditionAttribute knownConditionAttribute = (KnownConditionAttribute)base.GetType().GetCustomAttributes(typeof(KnownConditionAttribute), false)[0];
			base.TypeHash = knownConditionAttribute.Hash;
		}
		public override object Clone(PrototypeGame game)
		{
			Stream stream = new MemoryStream();
			BaseCondition.SerializeBaseCondition(game, stream, Endian.Little, this);
			stream.Position = 0L;
			ulong hash = stream.ReadValueU64();
			return BaseCondition.DeserializeBaseCondition(game, stream, Endian.Little, hash);
		}
		public static void SerializeBaseCondition(PrototypeGame game, Stream output, Endian endianess, BaseCondition condition)
		{
			if (game == PrototypeGame.P1)
			{
				P1Condition.SerializeBaseCondition(output, endianess, condition);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			P2Condition.SerializeBaseCondition(output, endianess, condition);
		}
		public static void SerializeBaseConditions(PrototypeGame game, Stream output, Endian endianess, List<BaseCondition> conditions)
		{
			if (game == PrototypeGame.P1)
			{
				P1Condition.SerializeBaseConditions(output, endianess, conditions);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			P2Condition.SerializeBaseConditions(output, endianess, conditions);
		}
		public static BaseCondition DeserializeBaseCondition(PrototypeGame game, Stream input, Endian endianess, ulong hash)
		{
			if (game == PrototypeGame.P1)
			{
				return P1Condition.DeserializeBaseCondition(input, endianess, hash);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return P2Condition.DeserializeBaseCondition(input, endianess, hash);
		}
		public static List<BaseCondition> DeserializeBaseConditions(PrototypeGame game, Stream input, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				return P1Condition.DeserializeBaseConditions(input, endianess);
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			return P2Condition.DeserializeBaseConditions(input, endianess);
		}
	}
}
