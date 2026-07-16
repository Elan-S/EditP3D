using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121305)]
    public class VertexAnimationSet : BaseNode
    {
        public uint Version { get; set; }
        public uint TargetCount { get; set; }
        public uint[] TargetIndices { get; set; }
        public uint[] TargetFlags { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.TargetCount + " targets)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.TargetCount);
            if (this.TargetIndices != null)
            {
                foreach (var value in this.TargetIndices)
                {
                    output.WriteValueU32(value);
                }
            }
            if (this.TargetFlags != null)
            {
                foreach (var value in this.TargetFlags)
                {
                    output.WriteValueU32(value);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.TargetCount = input.ReadValueU32();
            this.TargetIndices = new uint[this.TargetCount];
            for (int i = 0; i < this.TargetIndices.Length; i++)
            {
                this.TargetIndices[i] = input.ReadValueU32();
            }

            this.TargetFlags = new uint[this.TargetCount];
            for (int i = 0; i < this.TargetFlags.Length; i++)
            {
                this.TargetFlags[i] = input.ReadValueU32();
            }
        }
    }

    [KnownType(0x00121306)]
    public class VertexAnimationTarget : BaseNode
    {
        public uint Version { get; set; }
        public uint Index { get; set; }
        public uint Unknown2 { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Index + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.Index);
            output.WriteValueU32(this.Unknown2);
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Index = input.ReadValueU32();
            this.Unknown2 = input.ReadValueU32();
        }
    }

    [KnownType(0x00010F00)]
    public class VertexAnimationChannel : BaseNode
    {
        public uint Version { get; set; }
        public string ChannelType { get; set; }
        public uint Count { get; set; }
        public VertexAnimationChannelValue[] Values { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.ChannelType + ", " + this.Count + " values)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteString(this.ChannelType);
            output.WriteValueU32(this.Values == null ? 0 : (uint)this.Values.Length);
            if (this.Values != null)
            {
                foreach (var value in this.Values)
                {
                    output.WriteValueU32(value.VertexIndex);
                    value.Value.Serialize(output);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.ChannelType = input.ReadString(4);
            this.Count = input.ReadValueU32();
            this.Values = new VertexAnimationChannelValue[this.Count];
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = new VertexAnimationChannelValue
                {
                    VertexIndex = input.ReadValueU32(),
                    Value = new Vector3(input),
                };
            }
        }
    }

    public struct VertexAnimationChannelValue
    {
        public uint VertexIndex { get; set; }
        public Vector3 Value { get; set; }

        public override string ToString()
        {
            return this.VertexIndex + ": " + this.Value;
        }
    }
}
