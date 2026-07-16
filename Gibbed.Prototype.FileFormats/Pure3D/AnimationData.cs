using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x02F00000)]
    public class AnimationData : BaseNode
    {
        public uint Version { get; set; }
        public string Compression { get; set; }
        public uint UncompressedSize { get; set; }
        public uint CompressedSize { get; set; }
        public byte[] CompressedData { get; set; }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteString(this.Compression);
            output.WriteValueU32(this.UncompressedSize);
            output.WriteValueU32(this.CompressedSize);
            output.Write(this.CompressedData, 0, this.CompressedData == null ? 0 : this.CompressedData.Length);
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Compression = input.ReadString(4);
            this.UncompressedSize = input.ReadValueU32();
            this.CompressedSize = input.ReadValueU32();
            this.CompressedData = input.ReadBytes((int)this.CompressedSize);
        }
    }
}
