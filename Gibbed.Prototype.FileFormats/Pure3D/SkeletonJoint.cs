using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00023001)]
    public class SkeletonJoint : BaseNode
    {
        public string Name { get; set; }
        public int ParentIndex { get; set; }
        public Vector3 AxisX { get; set; }
        public Vector3 AxisY { get; set; }
        public Vector3 AxisZ { get; set; }
        public Vector3 Position { get; set; }
        public float Matrix03 { get; set; }
        public float Matrix13 { get; set; }
        public float Matrix23 { get; set; }
        public byte[] ExtraData { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? base.ToString() : base.ToString() + " (" + this.Name + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteStringAlignedU8(this.Name);
            output.WriteValueU32((uint)this.ParentIndex);
            this.AxisX.Serialize(output);
            output.WriteValueF32(this.Matrix03);
            this.AxisY.Serialize(output);
            output.WriteValueF32(this.Matrix13);
            this.AxisZ.Serialize(output);
            output.WriteValueF32(this.Matrix23);
            this.Position.Serialize(output);
            if (this.ExtraData != null)
            {
                output.Write(this.ExtraData, 0, this.ExtraData.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            var end = this.StartPosition + this.HeaderSize;
            this.Name = input.ReadStringAlignedU8();
            this.ParentIndex = (int)input.ReadValueU32();
            this.AxisX = new Vector3(input);
            this.Matrix03 = input.ReadValueF32();
            this.AxisY = new Vector3(input);
            this.Matrix13 = input.ReadValueF32();
            this.AxisZ = new Vector3(input);
            this.Matrix23 = input.ReadValueF32();
            this.Position = new Vector3(input);

            var remaining = (int)(end - input.Position);
            this.ExtraData = remaining > 0 ? input.ReadBytes(remaining) : new byte[0];
        }
    }
}
