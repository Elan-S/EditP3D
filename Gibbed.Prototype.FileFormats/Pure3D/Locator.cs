using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00014000)]
    public class Locator : BaseNode
    {
        public string Name { get; set; }
        public uint Version { get; set; }
        public Vector3 Position { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) == true
                       ? base.ToString()
                       : base.ToString() + " (" + this.Name + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteStringAlignedU8(this.Name);
            output.WriteValueU32(this.Version);
            (this.Position ?? new Vector3()).Serialize(output);
        }

        public override void Deserialize(Stream input)
        {
            this.Name = input.ReadStringAlignedU8();
            this.Version = input.ReadValueU32();
            this.Position = new Vector3(input);
        }
    }
}
