using System;
using System.IO;
using System.Runtime.Serialization;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats
{
	public class BufferItem
	{
		[DataMember(Name = "Buffer Type", Order = 1)]
		public DescriptionType BufferType { get; set; }

		[DataMember(Name = "Data", Order = 1)]
		public byte[] Data { get; set; }

		public override string ToString()
		{
			return "Buffer Item (" + this.BufferType.EnumValue.ToString() + ")";
		}

		BufferItem(Endian endianess)
		{
			this.endianess = endianess;
		}

		public object GetValue()
		{
			Stream input = new MemoryStream(this.Data);
			switch (this.BufferType.EnumValue)
			{
			case DescriptionTypeEnum.UV:
			case DescriptionTypeEnum.UV1:
				return new UVCoordinate(input, this.endianess);
			case DescriptionTypeEnum.Position:
			case DescriptionTypeEnum.Normal:
				return new Vector3(input, this.endianess);
			case DescriptionTypeEnum.Tangent:
				return new Vector4(input, this.endianess);
			case DescriptionTypeEnum.Weight:
			case DescriptionTypeEnum.Group:
			case DescriptionTypeEnum.Colour0:
				return this.Data;
			case DescriptionTypeEnum.Padding1:
				return new Vector2(input, this.endianess);
			}
			return this.Data;
		}

				public object GetValueP1()
		{
			Stream input = new MemoryStream(this.Data);
			switch (this.BufferType.EnumValue)
			{
			case DescriptionTypeEnum.UV:
				return new UVCoordinate(input, this.endianess);
			case DescriptionTypeEnum.Position:
			case DescriptionTypeEnum.Normal:
			case DescriptionTypeEnum.Weight:
				return new Vector3(input, this.endianess);
			case DescriptionTypeEnum.Tangent:
				return new Vector4(input, this.endianess);
			case DescriptionTypeEnum.Group:
				return this.Data;
			case DescriptionTypeEnum.Padding1:
				return new Vector2(input, this.endianess);
			}
			return this.Data;
		}

		public Endian endianess;
	}
}
