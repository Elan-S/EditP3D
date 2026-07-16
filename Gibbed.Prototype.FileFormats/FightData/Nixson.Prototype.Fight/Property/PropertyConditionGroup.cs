using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Nixson.Common;

namespace Nixson.Prototype.Fight.Property
{
	public class PropertyConditionGroup : BaseProperty
	{
		public List<BaseCondition> Conditions { get; set; } = new List<BaseCondition>();
		public PropertyConditionGroup()
		{
		}
		public PropertyConditionGroup(PropertyHash hash) : base(hash)
		{
		}
		public override object Clone(PrototypeGame game)
		{
			Stream stream = new MemoryStream();
			BaseProperty.SerializeBaseProperty(game, stream, Endian.Little, this);
			stream.Position = 0L;
			ulong hash = stream.ReadValueU64();
			stream.Position = 0L;
			return BaseProperty.DeserializeConditionProperty(game, stream, Endian.Little, (PropertyHash)hash);
		}
		public override void SerializeProperties(PrototypeGame game, Stream output, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				this.P1_SerializeProperties(output, endianess);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			this.P2_SerializeProperties(output, endianess);
		}
		public override void DeserializeProperties(PrototypeGame game, Stream input, Endian endianess)
		{
			if (game == PrototypeGame.P1)
			{
				this.P1_DeserializeProperties(input, endianess);
				return;
			}
			if (game != PrototypeGame.P2)
			{
				throw new Exception("Non valid game");
			}
			this.P2_DeserializeProperties(input, endianess);
		}
		private void P1_SerializeProperties(Stream output, Endian endianess)
		{
			BaseCondition.SerializeBaseConditions(PrototypeGame.P1, output, endianess, this.Conditions);
		}
		private void P1_DeserializeProperties(Stream input, Endian endianess)
		{
			this.Conditions = new List<BaseCondition>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				BaseCondition item = BaseCondition.DeserializeBaseCondition(PrototypeGame.P1, input, endianess, num);
				this.Conditions.Add(item);
			}
		}
		private void P2_SerializeProperties(Stream output, Endian endianess)
		{
			BaseCondition.SerializeBaseConditions(PrototypeGame.P2, output, endianess, this.Conditions);
		}
		private void P2_DeserializeProperties(Stream input, Endian endianess)
		{
			this.Conditions = new List<BaseCondition>();
			for (;;)
			{
				ulong num = input.ReadValueU64(endianess);
				if (num == 0UL)
				{
					break;
				}
				BaseCondition item = BaseCondition.DeserializeBaseCondition(PrototypeGame.P2, input, endianess, num);
				this.Conditions.Add(item);
			}
			input.Position -= 8L;
		}
	}
}
