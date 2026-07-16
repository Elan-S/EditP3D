using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121120)]
    [KnownType(0x00121121)]
    public class AnimationDataReference : BaseNode
    {
        public uint Version { get; set; }
        public uint Frames { get; set; }
        public uint Offset { get; set; }
        public byte[] ExtraData { get; set; }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.Frames);
            output.WriteValueU32(this.Offset);
            if (this.ExtraData != null)
            {
                output.Write(this.ExtraData, 0, this.ExtraData.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            var end = this.StartPosition + this.HeaderSize;
            this.Version = input.ReadValueU32();
            this.Frames = input.ReadValueU32();
            this.Offset = input.ReadValueU32();
            var remaining = (int)(end - input.Position);
            this.ExtraData = remaining > 0 ? input.ReadBytes(remaining) : new byte[0];
        }
    }
}
