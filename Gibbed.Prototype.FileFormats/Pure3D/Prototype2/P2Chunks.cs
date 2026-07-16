using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D.Prototype2
{
    [KnownType(0x07020000)]
    public class P2Skeleton : BaseNode
    {
        public string Name { get; set; }
        public uint Unknown1 { get; set; }
        public uint NumJoints { get; set; }
        public uint Unknown2 { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name)
                       ? "P2Skeleton"
                       : "P2Skeleton (" + this.Name.TrimEnd('\0') + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteStringAlignedU8(this.Name);
            output.WriteValueU32(this.Unknown1);
            output.WriteValueU32(this.NumJoints);
            output.WriteValueU32(this.Unknown2);
        }

        public override void Deserialize(Stream input)
        {
            this.Name = input.ReadStringAlignedU8();
            this.Unknown1 = input.ReadValueU32();
            this.NumJoints = input.ReadValueU32();
            this.Unknown2 = input.ReadValueU32();
        }
    }

    [KnownType(0x07021101)]
    public class P2SkeletonJoint : BaseNode
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name)
                       ? "P2SkeletonJoint"
                       : "P2SkeletonJoint (" + this.Name.TrimEnd('\0') + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteStringAlignedU8(this.Name);
            if (this.Data != null)
            {
                output.Write(this.Data, 0, this.Data.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            var end = this.StartPosition + this.HeaderSize;
            this.Name = input.ReadStringAlignedU8();
            var remaining = (int)(end - input.Position);
            this.Data = remaining > 0 ? input.ReadBytes(remaining) : new byte[0];
        }
    }

    [KnownType(0x07020001)]
    public class P2SkeletonRigData : BaseNode
    {
        public uint Count { get; set; }
        public byte[] Data { get; set; }

        public ushort[] SkinBoneIndices
        {
            get { return this.GetSkinBoneIndices(); }
        }

        public override string ToString()
        {
            return "P2SkeletonRigData (" + this.Count + " entries)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Count);
            if (this.Data != null)
            {
                output.Write(this.Data, 0, this.Data.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            var end = this.StartPosition + this.HeaderSize;
            this.Count = input.ReadValueU32();
            var remaining = (int)(end - input.Position);
            this.Data = remaining > 0 ? input.ReadBytes(remaining) : new byte[0];
        }

        public ushort[] GetSkinBoneIndices()
        {
            if (this.Data == null || this.Data.Length < 2 || this.Count == 0)
            {
                return new ushort[0];
            }

            var count = (int)Math.Min(this.Count + 1, (uint)(this.Data.Length / 2));
            var indices = new ushort[count];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = BitConverter.ToUInt16(this.Data, i * 2);
            }

            return indices;
        }
    }

    [KnownType(0x00025000)]
    public class P2PolySkinComposite : BaseNode
    {
        public uint Unknown1 { get; set; }
        public string Name { get; set; }
        public string SkeletonName { get; set; }
        public uint NumPolySkins { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? "P2PolySkinComposite" : "P2PolySkinComposite (" + this.Name.TrimEnd('\0') + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Unknown1);
            output.WriteStringAlignedU8(this.Name);
            output.WriteStringAlignedU8(this.SkeletonName);
            output.WriteValueU32(this.NumPolySkins);
        }

        public override void Deserialize(Stream input)
        {
            this.Unknown1 = input.ReadValueU32();
            this.Name = input.ReadStringAlignedU8();
            this.SkeletonName = input.ReadStringAlignedU8();
            this.NumPolySkins = input.ReadValueU32();
        }
    }

    [KnownType(0x00010040)]
    [KnownType(0x00010041)]
    public class P2Primitive : BaseNode
    {
        public uint Version { get; set; }
        public string Name { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint NumberOfVertices { get; set; }
        public uint Unknown3 { get; set; }
        public uint Unknown4 { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? base.ToString() : base.ToString() + " (" + this.Name + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteStringAlignedU8(this.Name);
            output.WriteValueU32(this.Unknown1);
            output.WriteValueU32(this.Unknown2);
            output.WriteValueU32(this.NumberOfVertices);
            output.WriteValueU32(this.Unknown3);
            output.WriteValueU32(this.Unknown4);
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Name = input.ReadStringAlignedU8();
            this.Unknown1 = input.ReadValueU32();
            this.Unknown2 = input.ReadValueU32();
            this.NumberOfVertices = input.ReadValueU32();
            this.Unknown3 = input.ReadValueU32();
            this.Unknown4 = input.ReadValueU32();
        }
    }

    [KnownType(0x00025001)]
    public class P2PolySkin : BaseNode
    {
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? "P2PolySkin" : "P2PolySkin (" + this.Name.TrimEnd('\0') + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Unknown1);
            output.WriteValueU32(this.Unknown2);
            output.WriteStringAlignedU8(this.Name);
        }

        public override void Deserialize(Stream input)
        {
            this.Unknown1 = input.ReadValueU32();
            this.Unknown2 = input.ReadValueU32();
            this.Name = input.ReadStringAlignedU8();
        }
    }

    [KnownType(0x00025002)]
    [KnownType(0x00025003)]
    public class P2PolySkinMetadata : BaseNode
    {
        public uint Unknown1 { get; set; }
        public string ShaderName { get; set; }
        public uint Unknown2 { get; set; }
        public string IndicesName { get; set; }
        public string VerticesName { get; set; }
        public string SkinName { get; set; }
        public byte Unknown3 { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.ShaderName)
                       ? base.ToString()
                       : base.ToString() + " (" + this.ShaderName + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Unknown1);
            output.WriteStringAlignedU8(this.ShaderName);
            output.WriteValueU32(this.Unknown2);
            output.WriteStringAlignedU8(this.IndicesName);
            output.WriteStringAlignedU8(this.VerticesName);
            output.WriteStringAlignedU8(this.SkinName);
            output.WriteByte(this.Unknown3);
        }

        public override void Deserialize(Stream input)
        {
            this.Unknown1 = input.ReadValueU32();
            this.ShaderName = input.ReadStringAlignedU8();
            this.Unknown2 = input.ReadValueU32();
            this.IndicesName = input.ReadStringAlignedU8();
            this.VerticesName = input.ReadStringAlignedU8();
            this.SkinName = input.ReadStringAlignedU8();
            this.Unknown3 = (byte)input.ReadByte();
        }
    }

    [KnownType(0x00010043)]
    public class P2BufferDescriptor : BaseNode
    {
        public uint AmountOfDescriptions { get; set; }
        public uint DescriptionSize { get; set; }
        public P2Description[] Descriptions { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.AmountOfDescriptions + " descriptions)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Descriptions == null ? 0 : (uint)this.Descriptions.Length);
            if (this.Descriptions == null)
            {
                return;
            }

            foreach (var description in this.Descriptions)
            {
                description.Serialize(output);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.AmountOfDescriptions = input.ReadValueU32();
            this.Descriptions = new P2Description[this.AmountOfDescriptions];
            for (int i = 0; i < this.Descriptions.Length; i++)
            {
                this.Descriptions[i] = new P2Description(input);
            }

            this.DescriptionSize = this.Descriptions.Length == 0 ? 0 : this.Descriptions[0].ItemSize;
        }
    }

    [KnownType(0x00010042)]
    public class P2Buffer : BaseNode
    {
        private P2BufferItem[] _Items;

        public uint BufferSize { get; set; }
        public byte[] Data { get; set; }

        [Browsable(false)]
        public P2BufferDescriptor Description
        {
            get { return this.ParentNode == null ? null : this.ParentNode.GetChildNode<P2BufferDescriptor>(); }
        }

        public P2BufferItem[] Items
        {
            get
            {
                if (this._Items == null)
                {
                    this._Items = this.ReadItems();
                }

                return this._Items;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " (" + this.BufferSize + " bytes)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Data == null ? 0 : (uint)this.Data.Length);
            if (this.Data != null)
            {
                output.Write(this.Data, 0, this.Data.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.BufferSize = input.ReadValueU32();
            this.Data = input.ReadBytes((int)this.BufferSize);
            this._Items = null;
        }

        private P2BufferItem[] ReadItems()
        {
            var description = this.Description;
            var primitive = this.ParentNode as P2Primitive;
            if (description == null ||
                description.Descriptions == null ||
                description.Descriptions.Length == 0 ||
                description.DescriptionSize == 0 ||
                primitive == null ||
                this.Data == null)
            {
                return new P2BufferItem[0];
            }

            var vertexCount = Math.Min((int)primitive.NumberOfVertices, this.Data.Length / (int)description.DescriptionSize);
            var items = new P2BufferItem[vertexCount * description.Descriptions.Length];
            for (int vertex = 0; vertex < vertexCount; vertex++)
            {
                var vertexOffset = vertex * (int)description.DescriptionSize;
                for (int i = 0; i < description.Descriptions.Length; i++)
                {
                    var current = description.Descriptions[i];
                    var nextOffset = i + 1 < description.Descriptions.Length
                                         ? description.Descriptions[i + 1].Offset
                                         : description.DescriptionSize;
                    var size = (int)(nextOffset - current.Offset);
                    var bytes = new byte[size];
                    Buffer.BlockCopy(this.Data, vertexOffset + (int)current.Offset, bytes, 0, size);
                    items[vertex * description.Descriptions.Length + i] = new P2BufferItem
                    {
                        VertexIndex = vertex,
                        BufferType = current.BufferType,
                        RawType = current.RawType,
                        ComponentType = current.Unknown1,
                        ComponentCount = current.Unknown2,
                        Data = bytes,
                    };
                }
            }

            return items;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class P2Description
    {
        public P2Description()
        {
        }

        public P2Description(Stream input)
        {
            this.Deserialize(input);
        }

        public string RawType { get; set; }
        public VertexDescriptionType BufferType { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Offset { get; set; }
        public uint ItemSize { get; set; }
        public uint BufferSize { get; set; }
        public uint Unknown5 { get; set; }
        public uint Unknown6 { get; set; }

        public override string ToString()
        {
            return this.BufferType + " offset " + this.Offset + " size " + this.ItemSize;
        }

        public void Serialize(Stream output)
        {
            output.WriteStringAlignedU8(ToP2Type(this.BufferType, this.RawType));
            output.WriteValueU32(this.Unknown1);
            output.WriteValueU32(this.Unknown2);
            output.WriteValueU32(this.Offset);
            output.WriteValueU32(this.ItemSize);
            output.WriteValueU32(this.BufferSize);
            output.WriteValueU32(this.Unknown5);
            output.WriteValueU32(this.Unknown6);
        }

        public void Deserialize(Stream input)
        {
            this.RawType = input.ReadStringAlignedU8();
            this.BufferType = FromP2Type(this.RawType);
            this.Unknown1 = input.ReadValueU32();
            this.Unknown2 = input.ReadValueU32();
            this.Offset = input.ReadValueU32();
            this.ItemSize = input.ReadValueU32();
            this.BufferSize = input.ReadValueU32();
            this.Unknown5 = input.ReadValueU32();
            this.Unknown6 = input.ReadValueU32();
        }

        private static VertexDescriptionType FromP2Type(string value)
        {
            switch ((value ?? string.Empty).TrimEnd('\0', ' ').ToLowerInvariant())
            {
                case "tex0":
                    return VertexDescriptionType.Uv0;
                case "tex1":
                    return VertexDescriptionType.Uv1;
                case "position":
                    return VertexDescriptionType.Position;
                case "normal":
                    return VertexDescriptionType.Normal;
                case "tangent":
                    return VertexDescriptionType.Tangent;
                case "weights":
                    return VertexDescriptionType.Weight;
                case "indices":
                    return VertexDescriptionType.Group;
                case "colour0":
                    return VertexDescriptionType.Colour0;
                case "pad":
                    return VertexDescriptionType.Padding1;
                default:
                    return VertexDescriptionType.Unknown;
            }
        }

        private static string ToP2Type(VertexDescriptionType type, string fallback)
        {
            switch (type)
            {
                case VertexDescriptionType.Uv0: return "tex0";
                case VertexDescriptionType.Uv1: return "tex1";
                case VertexDescriptionType.Position: return "position";
                case VertexDescriptionType.Normal: return "normal";
                case VertexDescriptionType.Tangent: return "tangent";
                case VertexDescriptionType.Weight: return "weights";
                case VertexDescriptionType.Group: return "indices";
                case VertexDescriptionType.Colour0: return "colour0";
                case VertexDescriptionType.Padding1: return "pad";
                default: return fallback ?? string.Empty;
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class P2BufferItem
    {
        public int VertexIndex { get; set; }
        public VertexDescriptionType BufferType { get; set; }
        public string RawType { get; set; }
        public uint ComponentType { get; set; }
        public uint ComponentCount { get; set; }
        public byte[] Data { get; set; }

        public int[] BoneIndices
        {
            get { return this.GetBoneIndices(); }
        }

        public Vector4 Weights
        {
            get { return this.GetWeights(); }
        }

        public override string ToString()
        {
            return this.BufferType + " vertex " + this.VertexIndex;
        }

        public Vector2 GetVector2()
        {
            using (var input = new MemoryStream(this.Data ?? new byte[0]))
            {
                return new Vector2(input);
            }
        }

        public Vector3 GetVector3()
        {
            using (var input = new MemoryStream(this.Data ?? new byte[0]))
            {
                return new Vector3(input);
            }
        }

        public Vector4 GetVector4()
        {
            using (var input = new MemoryStream(this.Data ?? new byte[0]))
            {
                return new Vector4(input);
            }
        }

        public Vector4 GetWeights()
        {
            if (this.BufferType != VertexDescriptionType.Weight || this.Data == null)
            {
                return null;
            }

            if (this.ComponentType == 0 && this.ComponentCount >= 4 && this.Data.Length >= 16)
            {
                return this.GetVector4();
            }

            if (this.Data.Length >= 16)
            {
                return this.GetVector4();
            }

            if (this.ComponentType == 3 && this.ComponentCount >= 4 && this.Data.Length >= 8)
            {
                return new Vector4
                {
                    X = BitConverter.ToUInt16(this.Data, 0) / 65535.0f,
                    Y = BitConverter.ToUInt16(this.Data, 2) / 65535.0f,
                    Z = BitConverter.ToUInt16(this.Data, 4) / 65535.0f,
                    W = BitConverter.ToUInt16(this.Data, 6) / 65535.0f,
                };
            }

            if (this.Data.Length >= 4)
            {
                return new Vector4
                {
                    X = this.Data[0] / 255.0f,
                    Y = this.Data[1] / 255.0f,
                    Z = this.Data[2] / 255.0f,
                    W = this.Data[3] / 255.0f,
                };
            }

            return null;
        }

        public int[] GetBoneIndices()
        {
            if (this.BufferType != VertexDescriptionType.Group || this.Data == null)
            {
                return new int[0];
            }

            if (this.ComponentType == 3 && this.ComponentCount >= 4 && this.Data.Length >= 8)
            {
                return new[]
                {
                    (int)BitConverter.ToUInt16(this.Data, 0),
                    (int)BitConverter.ToUInt16(this.Data, 2),
                    (int)BitConverter.ToUInt16(this.Data, 4),
                    (int)BitConverter.ToUInt16(this.Data, 6),
                };
            }

            if (this.ComponentCount >= 4 && this.Data.Length >= 4)
            {
                return new[]
                {
                    (int)this.Data[0],
                    (int)this.Data[1],
                    (int)this.Data[2],
                    (int)this.Data[3],
                };
            }

            if (this.Data.Length >= 8)
            {
                return new[]
                {
                    (int)BitConverter.ToUInt16(this.Data, 0),
                    (int)BitConverter.ToUInt16(this.Data, 2),
                    (int)BitConverter.ToUInt16(this.Data, 4),
                    (int)BitConverter.ToUInt16(this.Data, 6),
                };
            }

            return new int[0];
        }
    }
}
