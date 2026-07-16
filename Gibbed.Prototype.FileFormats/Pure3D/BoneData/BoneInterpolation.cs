using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D.BoneData
{
    [KnownType(0x00121110)]
    public class BoneInterpolation : BaseNode
    {
        public uint Version { get; set; }
        // public uint Unknown1 { get; set; }
        // public uint Unknown2 { get; set; }
        public uint Interpolate { get; set; }
 
        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.Interpolate);
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Interpolate = input.ReadValueU32();
        }
    }
}
