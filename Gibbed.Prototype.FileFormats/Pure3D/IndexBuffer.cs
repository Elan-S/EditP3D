using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00010013)]
    public class IndexBuffer : BaseNode
    {
        public uint Version { get; set; }
        public uint Param { get; set; }
        public uint BufferSize { get; set; }
        public Face[] Faces { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + this.Param + ", " + (this.Faces == null ? 0 : this.Faces.Length) + " faces)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteValueU32(this.Param);
            output.WriteValueU32(this.BufferSize);
            if (this.Faces != null)
            {
                foreach (var face in this.Faces)
                {
                    face.Serialize(output);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.Param = input.ReadValueU32();
            this.BufferSize = input.ReadValueU32();
            uint count = this.BufferSize / 6;
            this.Faces = new Face[count];
            for (int i = 0; i < this.Faces.Length; i++)
            {
                var face = new Face();
                face.Deserialize(input);
                this.Faces[i] = face;
            }
        }
    }

    public struct Face
    {
        public ushort Point1 { get; set; }
        public ushort Point2 { get; set; }
        public ushort Point3 { get; set; }

        public void Serialize(Stream output)
        {
            output.WriteValueU16(this.Point1);
            output.WriteValueU16(this.Point2);
            output.WriteValueU16(this.Point3);
        }

        public void Deserialize(Stream input)
        {
            this.Point1 = input.ReadValueU16();
            this.Point2 = input.ReadValueU16();
            this.Point3 = input.ReadValueU16();
        }
    }
}
