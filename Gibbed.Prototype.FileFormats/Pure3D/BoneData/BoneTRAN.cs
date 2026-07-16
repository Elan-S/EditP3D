using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    public class BoneTranslationData : BaseNode
    {
        public uint Version { get; set; }
        public FourCC Param { get; set; }
        public uint NumFrames { get; set; }
        public ushort[] Frames { get; set; }
        public Vector3[] Values { get; set; }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            this.Param.Serialize(output);
            output.WriteValueU32(this.NumFrames);
            for (int i = 0; i < this.NumFrames; i++)
            {
                output.WriteValueU16(this.Frames[i]);
            }
            for (int i = 0; i < this.NumFrames; i++)
            {
                this.Values[i].Serialize(output);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Param = new FourCC(input);
            this.NumFrames = input.ReadValueU32();

            this.Frames = new ushort[this.NumFrames];
            for (int i = 0; i < this.NumFrames; i++)
            {
                this.Frames[i] = input.ReadValueU16();
            }

            this.Values = new Vector3[this.NumFrames];
            for (int i = 0; i < this.NumFrames; i++)
            {
                this.Values[i] = new Vector3(input);
            }
        }
    }
}
