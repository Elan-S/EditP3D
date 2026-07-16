using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D.SpiderMan4
{
    public abstract class SM4RawPayloadNode : RawPayloadNode
    {
        public override string ToString()
        {
            return base.ToString() + " (Spider-Man 4)";
        }
    }

    public abstract class SM4OpaquePayloadNode : RawPayloadNode, OpaqueNode
    {
        public override void Deserialize(Stream input)
        {
            var length = (int)(this.StartPosition + this.HeaderSize - input.Position);
            this.Data = length > 0 ? input.ReadBytes(length) : new byte[0];
        }

        public override string ToString()
        {
            return base.ToString() + " (Spider-Man 4)";
        }
    }

    [KnownType(0x00010040)]
    [KnownType(0x00010041)]
    public class SM4Primitive : SM4RawPayloadNode
    {
    }

    [KnownType(0x00010042)]
    public class SM4Buffer : SM4RawPayloadNode
    {
    }

    [KnownType(0x00010043)]
    public class SM4BufferDescriptor : SM4RawPayloadNode
    {
    }

    [KnownType(0x00025000)]
    public class SM4PolySkinComposite : SM4RawPayloadNode
    {
    }

    [KnownType(0x00025001)]
    public class SM4PolySkin : SM4RawPayloadNode
    {
    }

    [KnownType(0x00025002)]
    [KnownType(0x00025003)]
    public class SM4PolySkinMetadata : SM4RawPayloadNode
    {
    }

    [KnownType(0x00121008)]
    [KnownType(0x00121009)]
    [KnownType(0x00121010)]
    public class SM4AnimationPayload : SM4OpaquePayloadNode
    {
    }

    [KnownType(0x00121000)]
    [KnownType(0x00121002)]
    [KnownType(0x00121006)]
    [KnownType(0x00121102)]
    [KnownType(0x00121103)]
    [KnownType(0x00121104)]
    [KnownType(0x00121105)]
    [KnownType(0x00121108)]
    [KnownType(0x0012110E)]
    [KnownType(0x00121110)]
    [KnownType(0x00121112)]
    [KnownType(0x00121114)]
    [KnownType(0x00121118)]
    [KnownType(0x00121119)]
    [KnownType(0x00121120)]
    [KnownType(0x00121121)]
    [KnownType(0x02F00000)]
    public class SM4LegacyAnimationPayload : SM4OpaquePayloadNode
    {
    }

    [KnownType(0x02F00001)]
    [KnownType(0x0900000A)]
    public class SM4Payload : SM4RawPayloadNode
    {
    }

    [KnownType(0x07021000)]
    [KnownType(0x07021004)]
    [KnownType(0x07021101)]
    [KnownType(0x07021801)]
    [KnownType(0x07021802)]
    [KnownType(0x07022000)]
    [KnownType(0x07022100)]
    [KnownType(0x07022101)]
    [KnownType(0x07022102)]
    [KnownType(0x07022107)]
    [KnownType(0x07022200)]
    public class SM4PhysicsPayload : SM4RawPayloadNode
    {
    }
}
