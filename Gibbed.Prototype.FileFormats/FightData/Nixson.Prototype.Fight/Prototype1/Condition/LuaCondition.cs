using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Condition
{
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownCondition(ConditionHash.Lua)]
	public class LuaCondition : P1Condition
	{
		public byte[] LuaCode
		{
			get
			{
				if (this._luaCode == null)
				{
					return new byte[0];
				}
				return this._luaCode;
			}
			set
			{
				this._luaCode = value;
			}
		}
		public bool IsLuaCompiled
		{
			get
			{
				return this.LuaCode[0] == 27 && this.LuaCode[1] == 76 && this.LuaCode[2] == 117 && this.LuaCode[3] == 97 && this.LuaCode[4] == 81;
			}
		}
		public uint LuaLength
		{
			get
			{
				if (this.LuaCode == null)
				{
					return 0U;
				}
				return (uint)this.LuaCode.Length;
			}
		}
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueU32(this.LuaLength);
			output.WriteBytes(this.LuaCode);
			uint num = this.LuaLength;
			while (num % 4U != 0U)
			{
				output.WriteByte(0);
				num += 1U;
			}
		}
		public override void Deserialize(Stream input, Endian endianess)
		{
			base.Deserialize(input, endianess);
			uint length = input.ReadValueU32(endianess);
			this.LuaCode = input.ReadBytes((int)length);
			uint num = this.LuaLength;
			while (num % 4U != 0U)
			{
				input.ReadByte();
				num += 1U;
			}
		}
		public byte[] _luaCode;
	}
}
