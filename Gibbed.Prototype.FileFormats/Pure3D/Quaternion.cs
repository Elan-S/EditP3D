using System.ComponentModel;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats
{
    public class Quaternion
    {
        public float W { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Quaternion()
        {
        }

        public Quaternion(Stream input)
        {
            this.Deserialize(input);
        }

        public void Serialize(Stream output)
        {
            output.WriteValueF32(this.X);
            output.WriteValueF32(this.Y);
            output.WriteValueF32(this.Z);
            output.WriteValueF32(this.W);
        }

        public void Deserialize(Stream input)
        {
            this.X = input.ReadValueF32();
            this.Y = input.ReadValueF32();
            this.Z = input.ReadValueF32();
            this.W = input.ReadValueF32();
        }
    }
}
