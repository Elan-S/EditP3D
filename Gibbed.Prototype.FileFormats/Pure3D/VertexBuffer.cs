using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00010012)]
    public class VertexBuffer : BaseNode
    {
        public uint Version { get; set; }
        public uint Param { get; set; }
        public uint BufferSize { get; set; }
        public byte[] RawData { get; set; }
        public VertexBufferItem[] BufferItems { get; set; }
        public VertexDescriptionList Description { get; private set; }

        public uint VertexCount
        {
            get
            {
                return this.Description == null || this.Description.VertexObjectSize == 0
                           ? 0
                           : this.BufferSize / this.Description.VertexObjectSize;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Param + ", " + this.BufferSize + " bytes)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.Param);
            output.WriteValueU32(this.BufferSize);
            if (this.BufferItems != null && this.BufferItems.Length > 0)
            {
                foreach (var item in this.BufferItems)
                {
                    output.Write(item.Data, 0, item.Data.Length);
                }
            }
            else if (this.RawData != null)
            {
                output.Write(this.RawData, 0, this.RawData.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Param = input.ReadValueU32();
            this.BufferSize = input.ReadValueU32();
            this.RawData = input.ReadBytes((int)this.BufferSize);
        }

        public void ResolveDescription()
        {
            if (this.Description != null)
            {
                return;
            }

            if (this.ParentNode == null)
            {
                return;
            }

            var description = this.ParentNode.Children
                                             .OfType<VertexDescriptionList>()
                                             .FirstOrDefault(d => d.Param == this.Param);
            if (description == null)
            {
                return;
            }

            this.Description = description;
            this.BufferItems = SliceBufferItems(description);
        }

        public VertexBufferItem GetItem(int vertexIndex, VertexDescriptionType type)
        {
            this.ResolveDescription();
            if (this.Description == null || this.BufferItems == null || this.Description.Descriptions == null)
            {
                return null;
            }

            int descriptionIndex = -1;
            for (int i = 0; i < this.Description.Descriptions.Length; i++)
            {
                if (this.Description.Descriptions[i].Type == type)
                {
                    descriptionIndex = i;
                    break;
                }
            }

            if (descriptionIndex < 0)
            {
                return null;
            }

            int index = vertexIndex * this.Description.Descriptions.Length + descriptionIndex;
            return index >= 0 && index < this.BufferItems.Length ? this.BufferItems[index] : null;
        }

        public bool HasElement(VertexDescriptionType type)
        {
            this.ResolveDescription();
            return this.Description != null &&
                   this.Description.Descriptions != null &&
                   this.Description.Descriptions.Any(d => d.Type == type);
        }

        public Vector2 GetVector2(int vertexIndex, VertexDescriptionType type)
        {
            var item = this.GetItem(vertexIndex, type);
            return item == null ? null : item.GetVector2();
        }

        public VertexUv GetUv(int vertexIndex, VertexDescriptionType type)
        {
            var item = this.GetItem(vertexIndex, type);
            return item == null ? null : item.GetUv();
        }

        public Vector3 GetVector3(int vertexIndex, VertexDescriptionType type)
        {
            var item = this.GetItem(vertexIndex, type);
            return item == null ? null : item.GetVector3();
        }

        public Vector4 GetVector4(int vertexIndex, VertexDescriptionType type)
        {
            var item = this.GetItem(vertexIndex, type);
            return item == null ? null : item.GetVector4();
        }

        public byte[] GetBytes(int vertexIndex, VertexDescriptionType type)
        {
            var item = this.GetItem(vertexIndex, type);
            return item == null ? null : item.GetBytes();
        }

        private VertexBufferItem[] SliceBufferItems(VertexDescriptionList description)
        {
            if (description.VertexObjectSize == 0)
            {
                return new VertexBufferItem[0];
            }

            uint vertexCount = this.BufferSize / description.VertexObjectSize;
            var items = new VertexBufferItem[vertexCount * description.AmountOfDescriptions];
            for (int vertex = 0; vertex < vertexCount; vertex++)
            {
                int vertexOffset = vertex * (int)description.VertexObjectSize;
                for (int element = 0; element < description.Descriptions.Length; element++)
                {
                    var vertexDescription = description.Descriptions[element];
                    int itemSize = (int)vertexDescription.Size;
                    var data = new byte[itemSize];
                    if (itemSize > 0)
                    {
                        Buffer.BlockCopy(this.RawData, vertexOffset + (int)vertexDescription.Offset, data, 0, itemSize);
                    }

                    items[vertex * description.Descriptions.Length + element] = new VertexBufferItem
                    {
                        Owner = this,
                        Description = vertexDescription,
                        VertexIndex = vertex,
                        ElementIndex = element,
                        Data = data,
                    };
                }
            }

            return items;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class VertexBufferItem
    {
        [Browsable(false)]
        public VertexBuffer Owner { get; set; }

        public int VertexIndex { get; set; }
        public int ElementIndex { get; set; }
        public VertexDescription Description { get; set; }
        public byte[] Data { get; set; }

        [Browsable(false)]
        public string[] BoneNames { get; private set; }

        [Browsable(false)]
        public VertexDescriptionType Type
        {
            get { return this.Description == null ? VertexDescriptionType.Unknown : this.Description.Type; }
        }

        public object Value
        {
            get { return this.GetValueP1(); }
        }

        public VertexWeightInfluence[] WeightInfluences
        {
            get
            {
                if (this.Type != VertexDescriptionType.Weight)
                {
                    return new VertexWeightInfluence[0];
                }

                var weight = this.GetVector3();
                if (weight == null)
                {
                    return new VertexWeightInfluence[0];
                }

                return new[]
                {
                    new VertexWeightInfluence(0, weight.X, GetBoneName(0)),
                    new VertexWeightInfluence(1, weight.Y, GetBoneName(1)),
                    new VertexWeightInfluence(2, weight.Z, GetBoneName(2)),
                };
            }
        }

        public void SetBoneNames(string[] boneNames)
        {
            this.BoneNames = boneNames == null ? null : (string[])boneNames.Clone();
        }

        public object GetValueP1()
        {
            if (this.Description == null)
            {
                return this.GetBytes();
            }

            switch (this.Description.Type)
            {
                case VertexDescriptionType.Uv0:
                case VertexDescriptionType.Uv1:
                case VertexDescriptionType.Uv2:
                    return this.GetUv();
                case VertexDescriptionType.Position:
                case VertexDescriptionType.Normal:
                case VertexDescriptionType.Weight:
                case VertexDescriptionType.VertexMorphPositionOffset:
                case VertexDescriptionType.VertexMorphNormalOffset:
                    return this.GetVector3();
                case VertexDescriptionType.Tangent:
                    return this.GetVector4();
                case VertexDescriptionType.Group:
                case VertexDescriptionType.Colour0:
                    return this.GetBytes();
                case VertexDescriptionType.UvPadding1:
                case VertexDescriptionType.Padding1:
                    return this.GetVector2();
                default:
                    return this.GetBytes();
            }
        }

        public override string ToString()
        {
            var label = this.Type.ToString();
            if (this.Type == VertexDescriptionType.Weight)
            {
                return label + " vertex " + this.VertexIndex.ToString(CultureInfo.InvariantCulture) + " = " + FormatWeights(this.GetVector3(), this.BoneNames);
            }

            return label + " vertex " + this.VertexIndex.ToString(CultureInfo.InvariantCulture) + " = " + FormatValue(this.GetValueP1());
        }

        public Vector2 GetVector2()
        {
            return this.Data == null || this.Data.Length < 8 ? null : this.ReadVector2(false);
        }

        public VertexUv GetUv()
        {
            if (this.Data == null || this.Data.Length < 8)
            {
                return null;
            }

            using (var input = new MemoryStream(this.Data))
            {
                return new VertexUv
                {
                    U = input.ReadValueF32(),
                    V = input.ReadValueF32(),
                };
            }
        }

        public Vector3 GetVector3()
        {
            if (this.Data == null || this.Data.Length < 12)
            {
                return null;
            }

            using (var input = new MemoryStream(this.Data))
            {
                return new Vector3(input);
            }
        }

        public Vector4 GetVector4()
        {
            if (this.Data == null || this.Data.Length < 16)
            {
                return null;
            }

            using (var input = new MemoryStream(this.Data))
            {
                return new Vector4(input);
            }
        }

        public byte[] GetBytes()
        {
            return this.Data == null ? null : (byte[])this.Data.Clone();
        }

        private Vector2 ReadVector2(bool flipV)
        {
            using (var input = new MemoryStream(this.Data))
            {
                var value = new Vector2(input);
                if (flipV)
                {
                    value.Y = 1.0f - value.Y;
                }
                return value;
            }
        }

        private string GetBoneName(int index)
        {
            return this.BoneNames != null && index >= 0 && index < this.BoneNames.Length ? this.BoneNames[index] : null;
        }

        private static string FormatValue(object value)
        {
            var vector2 = value as Vector2;
            if (vector2 != null)
            {
                return FormatFloat(vector2.X) + ", " + FormatFloat(vector2.Y);
            }

            var uv = value as VertexUv;
            if (uv != null)
            {
                return FormatFloat(uv.U) + ", " + FormatFloat(uv.V);
            }

            var vector3 = value as Vector3;
            if (vector3 != null)
            {
                return FormatFloat(vector3.X) + ", " + FormatFloat(vector3.Y) + ", " + FormatFloat(vector3.Z);
            }

            var vector4 = value as Vector4;
            if (vector4 != null)
            {
                return FormatFloat(vector4.X) + ", " + FormatFloat(vector4.Y) + ", " + FormatFloat(vector4.Z) + ", " + FormatFloat(vector4.W);
            }

            var bytes = value as byte[];
            if (bytes != null)
            {
                return bytes.Length.ToString(CultureInfo.InvariantCulture) + " bytes";
            }

            return value == null ? string.Empty : value.ToString();
        }

        private static string FormatWeights(Vector3 weights, string[] boneNames)
        {
            if (weights == null)
            {
                return string.Empty;
            }

            var values = new[]
            {
                FormatWeight(weights.X, boneNames, 0),
                FormatWeight(weights.Y, boneNames, 1),
                FormatWeight(weights.Z, boneNames, 2),
            };

            return string.Join(", ", values);
        }

        private static string FormatWeight(float value, string[] boneNames, int index)
        {
            var text = FormatFloat(value);
            if (boneNames != null && index >= 0 && index < boneNames.Length && string.IsNullOrEmpty(boneNames[index]) == false)
            {
                text += " (" + boneNames[index] + ")";
            }

            return text;
        }

        private static string FormatFloat(float value)
        {
            return value.ToString("0.######", CultureInfo.InvariantCulture);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class VertexUv
    {
        public float U { get; set; }
        public float V { get; set; }

        public override string ToString()
        {
            return this.U.ToString("0.######", CultureInfo.InvariantCulture) + ", " +
                   this.V.ToString("0.######", CultureInfo.InvariantCulture);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class VertexWeightInfluence
    {
        public VertexWeightInfluence(int index, float value, string boneName)
        {
            this.Index = index;
            this.Value = value;
            this.BoneName = boneName;
        }

        public int Index { get; private set; }
        public float Value { get; private set; }

        [ReadOnly(true)]
        public string BoneName { get; private set; }

        public override string ToString()
        {
            var text = this.Value.ToString("0.######", CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(this.BoneName) == false)
            {
                text += " (" + this.BoneName + ")";
            }

            return text;
        }
    }
}
