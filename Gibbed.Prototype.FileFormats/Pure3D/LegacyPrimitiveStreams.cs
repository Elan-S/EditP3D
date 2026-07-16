using System;
using System.ComponentModel;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    public abstract class LegacyCountedNode : BaseNode
    {
        public uint Count { get; set; }
    }

    public abstract class LegacyVector3Stream : LegacyCountedNode
    {
        public Vector3[] Values { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Values == null ? 0 : this.Values.Length) + " values)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Values == null ? 0 : (uint)this.Values.Length);
            if (this.Values != null)
            {
                foreach (var value in this.Values)
                {
                    value.Serialize(output);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Values = new Vector3[this.Count];
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = new Vector3(input);
            }
        }
    }

    [KnownType(0x00010005)]
    public class LegacyPositionStream : LegacyVector3Stream
    {
    }

    [KnownType(0x00010006)]
    public class LegacyNormalStream : LegacyVector3Stream
    {
    }

    [KnownType(0x00010008)]
    public class LegacyColourStream : LegacyCountedNode
    {
        public uint[] Values { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Values == null ? 0 : this.Values.Length) + " colours)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Values == null ? 0 : (uint)this.Values.Length);
            if (this.Values != null)
            {
                foreach (var value in this.Values)
                {
                    output.WriteValueU32(value);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Values = new uint[this.Count];
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = input.ReadValueU32();
            }
        }
    }

    [KnownType(0x00010010)]
    public class LegacyByteStream : LegacyCountedNode
    {
        public byte[] Values { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Values == null ? 0 : this.Values.Length) + " values)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Values == null ? 0 : (uint)this.Values.Length);
            if (this.Values != null)
            {
                output.Write(this.Values, 0, this.Values.Length);
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Values = input.ReadBytes((int)this.Count);
        }
    }

    [KnownType(0x0001000C)]
    public class LegacyWeightStream : LegacyVector3Stream
    {
    }

    [KnownType(0x00010028)]
    public class LegacyTangentStream : LegacyCountedNode
    {
        public Vector4[] Values { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Values == null ? 0 : this.Values.Length) + " values)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Values == null ? 0 : (uint)this.Values.Length);
            if (this.Values != null)
            {
                foreach (var value in this.Values)
                {
                    value.Serialize(output);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Values = new Vector4[this.Count];
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = new Vector4(input);
            }
        }
    }

    [KnownType(0x00010007)]
    public class LegacyUvStream : LegacyCountedNode
    {
        public Vector2[] Values { get; set; }
        public byte[] Padding { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Values == null ? 0 : this.Values.Length) + " values)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Values == null ? 0 : (uint)this.Values.Length);
            if (this.Padding != null && this.Padding.Length > 0)
            {
                output.Write(this.Padding, 0, this.Padding.Length);
            }
            if (this.Values != null)
            {
                foreach (var value in this.Values)
                {
                    value.Serialize(output);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Padding = input.ReadBytes(4);
            this.Values = new Vector2[this.Count];
            for (int i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = new Vector2(input);
            }

            var end = this.StartPosition + this.HeaderSize;
            if (input.Position != end)
            {
                throw new FormatException("legacy UV stream did not consume the expected payload");
            }
        }
    }

    [KnownType(0x0001000A)]
    public class LegacyIndexStream : LegacyCountedNode
    {
        public uint[] Indices { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Indices == null ? 0 : this.Indices.Length) + " indices)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Indices == null ? 0 : (uint)this.Indices.Length);
            if (this.Indices != null)
            {
                foreach (var index in this.Indices)
                {
                    output.WriteValueU32(index);
                }
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

    [KnownType(0x0001000B)]
    public class LegacyGroupStream : LegacyCountedNode
    {
        public LegacyGroupValue[] Groups { get; set; }

        public override string ToString()
        {
            return base.ToString() + " (" + (this.Groups == null ? 0 : this.Groups.Length) + " groups)";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Groups == null ? 0 : (uint)this.Groups.Length);
            if (this.Groups != null)
            {
                foreach (var group in this.Groups)
                {
                    output.WriteValueU8(group.A);
                    output.WriteValueU8(group.B);
                    output.WriteValueU8(group.C);
                    output.WriteValueU8(group.D);
                }
            }
        }

        public override void Deserialize(Stream input)
        {
            this.Count = input.ReadValueU32();
            this.Groups = new LegacyGroupValue[this.Count];
            for (int i = 0; i < this.Groups.Length; i++)
            {
                this.Groups[i] = new LegacyGroupValue
                {
                    A = input.ReadValueU8(),
                    B = input.ReadValueU8(),
                    C = input.ReadValueU8(),
                    D = input.ReadValueU8(),
                };
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct LegacyGroupValue
    {
        public byte A { get; set; }
        public byte B { get; set; }
        public byte C { get; set; }
        public byte D { get; set; }

        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return this.A;
                    case 1: return this.B;
                    case 2: return this.C;
                    case 3: return this.D;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public override string ToString()
        {
            return this.A + ", " + this.B + ", " + this.C + ", " + this.D;
        }
    }
}
