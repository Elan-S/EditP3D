using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    public enum VertexDescriptionType
    {
        Unknown,
        Position,
        Normal,
        Tangent,
        Weight,
        Group,
        Uv0,
        Uv1,
        Uv2,
        UvPadding1,
        Colour0,
        Padding1,
        VertexMorphPositionOffset,
        VertexMorphNormalOffset,
    }

    public class VertexDescription
    {
        public uint RawType { get; set; }
        public VertexDescriptionType Type { get; set; }
        public uint Unknown1 { get; set; }
        public uint Offset { get; set; }
        public uint VertexObjectSize { get; set; }
        public uint Unknown2 { get; set; }

        public uint Size { get; set; }

        public void Serialize(Stream output)
        {
            output.WriteValueU32(this.RawType);
            output.WriteValueU32(this.Unknown1);
            output.WriteValueU32(this.Offset);
            output.WriteByte((byte)this.VertexObjectSize);
            output.WriteValueU32(this.Unknown2);
        }

        public void Deserialize(Stream input)
        {
            this.RawType = input.ReadValueU32();
            this.Type = GetType(this.RawType);
            this.Unknown1 = input.ReadValueU32();
            this.Offset = input.ReadValueU32();
            this.VertexObjectSize = (uint)input.ReadByte();
            this.Unknown2 = input.ReadValueU32();
        }

        private static VertexDescriptionType GetType(uint value)
        {
            switch (value)
            {
                case 0x00364509:
                    return VertexDescriptionType.Uv0;
                case 0x0036450A:
                    return VertexDescriptionType.Uv1;
                case 0x0036450B:
                    return VertexDescriptionType.Uv2;
                case 0x2C929929:
                    return VertexDescriptionType.Position;
                case 0x3898FC04:
                    return VertexDescriptionType.Colour0;
                case 0x49570CFB:
                    return VertexDescriptionType.Weight;
                case 0x73D5CBA7:
                    return VertexDescriptionType.Group;
                case 0xA4176245:
                    return VertexDescriptionType.Tangent;
                case 0xC206BCE7:
                    return VertexDescriptionType.Normal;
                case 0x9D6FC424:
                    return VertexDescriptionType.VertexMorphPositionOffset;
                case 0x9C3F1C78:
                    return VertexDescriptionType.VertexMorphNormalOffset;
                default:
                    return VertexDescriptionType.Unknown;
            }
        }
    }
}
