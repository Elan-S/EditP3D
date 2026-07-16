using System;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    public abstract class RawPayloadNode : BaseNode
    {
        public byte[] Data { get; set; }

        public override void Serialize(Stream output)
        {
            if (this.Data != null)
            {
                output.Write(this.Data, 0, this.Data.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            var length = (int)(this.StartPosition + this.HeaderSize - input.Position);
            this.Data = length > 0 ? input.ReadBytes(length) : new byte[0];
        }
    }

    [KnownType(0x00010003)]
    public class LegacyBoundingBox : BaseNode
    {
        public Vector3 Minimum { get; set; }
        public Vector3 Maximum { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Minimum + " - " + this.Maximum + ")";
        }

        public override void Serialize(Stream output)
        {
            this.Minimum.Serialize(output);
            this.Maximum.Serialize(output);
        }

        public override void Deserialize(Stream input)
        {
            this.Minimum = new Vector3(input);
            this.Maximum = new Vector3(input);
        }
    }

    [KnownType(0x00010004)]
    public class LegacyBoundingSphere : BaseNode
    {
        public Vector3 Centre { get; set; }
        public float Radius { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Centre + ", r=" + this.Radius + ")";
        }

        public override void Serialize(Stream output)
        {
            this.Centre.Serialize(output);
            output.WriteValueF32(this.Radius);
        }

        public override void Deserialize(Stream input)
        {
            this.Centre = new Vector3(input);
            this.Radius = input.ReadValueF32();
        }
    }

    [KnownType(0x00010011)]
    public class LegacyPrimitiveGroupFlags : RawPayloadNode
    {
        public byte Flags
        {
            get { return this.Data != null && this.Data.Length > 0 ? this.Data[0] : (byte)0; }
        }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Flags + ")";
        }
    }

    [KnownType(0x00010017)]
    public class LegacyPrimitiveGroupMeta : RawPayloadNode
    {
    }

    [KnownType(0x00010019)]
    public class LegacyPrimitiveGroup : RawPayloadNode
    {
    }

    [KnownType(0x0001001B)]
    public class LegacyIndexRecordStream : RawPayloadNode
    {
        public uint Count
        {
            get { return this.Data != null && this.Data.Length >= 4 ? BitConverter.ToUInt32(this.Data, 0) : 0; }
        }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Count + " records)";
        }
    }

    [KnownType(0x00010021)]
    public class LegacySkinInfo : RawPayloadNode
    {
    }

    [KnownType(0x00011000)]
    public class LegacyShader : BaseNode
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }
        public string TemplateName { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint ParamCount { get; set; }
        public byte[] ExtraData { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) == true
                       ? base.ToString()
                       : base.ToString() + " (" + this.Name + ", " + this.TemplateName + ")";
        }

        public override void Serialize(Stream output)
        {
            if (this.Data != null && this.Data.Length > 0)
            {
                output.Write(this.Data, 0, this.Data.Length);
                return;
            }

            output.WriteStringAlignedU8(this.Name);
            output.WriteStringAlignedU8(this.TemplateName);
            output.WriteValueU32(this.Unknown1);
            output.WriteValueU32(this.Unknown2);
            output.WriteValueU32(this.ParamCount);
            if (this.ExtraData != null)
            {
                output.Write(this.ExtraData, 0, this.ExtraData.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            var end = this.StartPosition + this.HeaderSize;
            this.Data = input.ReadBytes((int)(end - input.Position));
            using (var temp = new MemoryStream(this.Data))
            {
                this.Name = temp.ReadStringAlignedU8();
                this.TemplateName = temp.ReadStringAlignedU8();
                this.Unknown1 = temp.ReadValueU32();
                this.Unknown2 = temp.ReadValueU32();
                this.ParamCount = temp.ReadValueU32();
                var remaining = (int)(temp.Length - temp.Position);
                this.ExtraData = remaining > 0 ? temp.ReadBytes(remaining) : new byte[0];
            }
        }
    }

    public abstract class LegacyShaderParameter : RawPayloadNode
    {
        public FourCC Name { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Name + ")";
        }

        public override void Serialize(Stream output)
        {
            this.Name.Serialize(output);
            if (this.Data != null)
            {
                output.Write(this.Data, 0, this.Data.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Name = new FourCC(input);
            var length = (int)(this.StartPosition + this.HeaderSize - input.Position);
            this.Data = length > 0 ? input.ReadBytes(length) : new byte[0];
        }
    }

    [KnownType(0x00011002)]
    public class LegacyShaderTextureParameter : LegacyShaderParameter
    {
    }

    [KnownType(0x00011003)]
    public class LegacyShaderIntParameter : LegacyShaderParameter
    {
    }

    [KnownType(0x00011004)]
    public class LegacyShaderFloatParameter : LegacyShaderParameter
    {
    }

    [KnownType(0x00011005)]
    public class LegacyShaderColourParameter : LegacyShaderParameter
    {
    }

    [KnownType(0x00023002)]
    public class SkeletonPartition : RawPayloadNode
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) == true ? base.ToString() : base.ToString() + " (" + this.Name + ")";
        }

        public override void Serialize(Stream output)
        {
            if (this.Data != null)
            {
                output.Write(this.Data, 0, this.Data.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            base.Deserialize(input);
            using (var temp = new MemoryStream(this.Data))
            {
                try
                {
                    this.Name = temp.ReadStringAlignedU8();
                }
                catch
                {
                    this.Name = null;
                }
            }
        }
    }

    [KnownType(0x00023003)]
    public class SkeletonLimb : SkeletonPartition
    {
    }

    [KnownType(0x00122000)]
    public class SortOrder : RawPayloadNode
    {
    }
}
