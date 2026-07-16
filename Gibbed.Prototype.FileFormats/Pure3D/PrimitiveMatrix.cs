using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x0001000D)]
    public class PrimitiveMatrix : BaseNode
    {
        public uint Count { get; set; }
        public uint[] Indices { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Count + " indices)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Count);
            if (this.Indices == null)
            {
                return;
            }

            for (int i = 0; i < this.Indices.Length; i++)
            {
                output.WriteValueU32(this.Indices[i]);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Indices = new uint[this.Count];
            for (int i = 0; i < this.Indices.Length; i++)
            {
                this.Indices[i] = input.ReadValueU32();
            }
        }
    }
}
