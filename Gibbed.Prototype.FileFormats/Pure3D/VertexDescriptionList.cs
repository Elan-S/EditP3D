using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00010014)]
    public class VertexDescriptionList : BaseNode
    {
        public uint Version { get; set; }
        public uint Param { get; set; }
        public uint BufferSize { get; set; }
        public uint DescriptionSize { get; set; }
        public uint AmountOfDescriptions { get; set; }
        public VertexDescription[] Descriptions { get; set; }
        public VertexDescriptionListLayout Layout { get; set; }
        public byte[] LegacyData { get; set; }

        public uint LegacyType { get; set; }
        public uint LegacyParam { get; set; }
        public uint LegacyVertexObjectSize { get; set; }
        public uint LegacyUnknown1 { get; set; }
        public uint LegacyUnknown2 { get; set; }
        public uint LegacyUnknown3 { get; set; }
        public uint LegacyVertexType { get; set; }

        public uint VertexObjectSize
        {
            get
            {
                if (this.Layout == VertexDescriptionListLayout.Legacy)
                {
                    return this.LegacyVertexObjectSize;
                }

                return this.Descriptions == null || this.Descriptions.Length == 0
                           ? 0
                           : this.Descriptions[0].VertexObjectSize;
            }
        }

        public override string ToString()
        {
            if (this.Layout == VertexDescriptionListLayout.Legacy)
            {
                return base.ToString() + " (legacy, vertex size " + this.LegacyVertexObjectSize + ")";
            }

            return base.ToString() + " (" + this.Param + ", " + this.AmountOfDescriptions + " elements)";
        }

        public override void Serialize(Stream output)
        {
            if (this.Layout == VertexDescriptionListLayout.Legacy)
            {
                if (this.LegacyData != null && this.LegacyData.Length > 0)
                {
                    output.Write(this.LegacyData, 0, this.LegacyData.Length);
                    return;
                }

                output.WriteValueU32(this.LegacyType);
                output.WriteValueU32(this.LegacyParam);
                output.WriteValueU32(this.LegacyVertexObjectSize);
                output.WriteValueU32(this.LegacyUnknown1);
                output.WriteValueU32(this.LegacyUnknown2);
                output.WriteValueU32(this.LegacyUnknown3);
                output.WriteValueU32(this.LegacyVertexType);
                return;
            }

            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.Param);
            output.WriteValueU32(this.BufferSize);
            output.WriteValueU32(this.DescriptionSize);
            if (this.Descriptions != null)
            {
                foreach (var description in this.Descriptions)
                {
                    description.Serialize(output);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            var end = this.StartPosition + this.HeaderSize;
            var payloadSize = this.HeaderSize - 12;
            if (payloadSize == 28)
            {
                this.Layout = VertexDescriptionListLayout.Legacy;
                this.LegacyData = input.ReadBytes((int)payloadSize);
                using (var legacy = new MemoryStream(this.LegacyData))
                {
                    this.LegacyType = legacy.ReadValueU32();
                    this.LegacyParam = legacy.ReadValueU32();
                    this.LegacyVertexObjectSize = legacy.ReadValueU32();
                    this.LegacyUnknown1 = legacy.ReadValueU32();
                    this.LegacyUnknown2 = legacy.ReadValueU32();
                    this.LegacyUnknown3 = legacy.ReadValueU32();
                    this.LegacyVertexType = legacy.ReadValueU32();
                }

                this.Version = this.LegacyType;
                this.Param = this.LegacyParam;
                this.BufferSize = 0;
                this.DescriptionSize = payloadSize;
                this.AmountOfDescriptions = 0;
                this.Descriptions = new VertexDescription[0];
                return;
            }

            this.Layout = VertexDescriptionListLayout.Prototype1;
            this.Version = input.ReadValueU32();
            this.Param = input.ReadValueU32();
            this.BufferSize = input.ReadValueU32();
            this.DescriptionSize = input.ReadValueU32();
            if (this.DescriptionSize > end - input.Position ||
                this.DescriptionSize % 17 != 0)
            {
                throw new InvalidDataException("unsupported vertex description list layout");
            }

            this.AmountOfDescriptions = this.DescriptionSize / 17;
            this.Descriptions = new VertexDescription[this.AmountOfDescriptions];
            for (int i = 0; i < this.Descriptions.Length; i++)
            {
                var description = new VertexDescription();
                description.Deserialize(input);
                this.Descriptions[i] = description;
            }

            for (int i = 0; i < this.Descriptions.Length; i++)
            {
                uint nextOffset = i + 1 < this.Descriptions.Length
                                      ? this.Descriptions[i + 1].Offset
                                      : this.VertexObjectSize;
                this.Descriptions[i].Size = nextOffset > this.Descriptions[i].Offset
                                                ? nextOffset - this.Descriptions[i].Offset
                                                : 0;
            }
        }
    }

    public enum VertexDescriptionListLayout
    {
        Prototype1,
        Legacy,
    }
}
