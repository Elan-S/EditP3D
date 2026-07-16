using System;
using System.IO;
using Gibbed.IO;

namespace Nixson.Prototype.Fight.Prototype1.Track
{
	[KnownNodeForContext(ContextHash.Motion)]
	[KnownNodeForContext(ContextHash.Scenario)]
	[KnownTrack(TrackHash.Lua)]
	public class LuaTrack : P1Track
	{
		public float StartTime { get; set; }
		public float EndTime { get; set; }
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
				return this.LuaCode != null && this.LuaCode.Length != 0 && (this.LuaCode[0] == 27 && this.LuaCode[1] == 76 && this.LuaCode[2] == 117 && this.LuaCode[3] == 97) && this.LuaCode[4] == 81;
			}
		}
		public uint LuaLength
		{
			get
			{
				if (this.LuaCode != null)
				{
					return (uint)this.LuaCode.Length;
				}
				return 0U;
			}
		}
		public override void Serialize(Stream output, Endian endianess)
		{
			base.Serialize(output, endianess);
			output.WriteValueF32(this.StartTime, endianess);
			output.WriteValueF32(this.EndTime, endianess);
			output.WriteValueU32(this.LuaLength, endianess);
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
			this.StartTime = input.ReadValueF32(endianess);
			this.EndTime = input.ReadValueF32(endianess);
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
