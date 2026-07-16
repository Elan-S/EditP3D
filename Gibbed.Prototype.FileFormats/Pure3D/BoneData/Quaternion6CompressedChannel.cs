using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121112)]
    public class Quaternion6CompressedChannel : AnimationChannel
    {
        public override void Serialize(Stream output)
        {
            base.Serialize(output);
            output.WriteValueU32(this.NumberOfFrames);
            if (!this.ContainsAnimData)
            {
                return;
            }

            foreach (var frame in this.GetRawFrameKeys())
            {
                output.WriteValueU16(frame);
            }

            foreach (var frame in this.GetRawFrameKeys())
            {
                var value = this.Frames[frame];
                output.WriteValueS16((short)(value.X * 32767.0f));
                output.WriteValueS16((short)(value.Y * 32767.0f));
                output.WriteValueS16((short)(value.Z * 32767.0f));
            }
        }

        public override void Deserialize(Stream input)
        {
            base.Deserialize(input);
            this.NumberOfFrames = input.ReadValueU32();
            this.Frames = new Dictionary<ushort, Vector4>();
            this.FrameOrder = new ushort[this.NumberOfFrames];
            for (int i = 0; i < this.NumberOfFrames; i++)
            {
                var frame = input.ReadValueU16();
                this.FrameOrder[i] = frame;
                this.AddFrame(frame);
            }

            this.ReadChannel(input);
        }

        protected override void ReadChannel(Stream input)
        {
            foreach (var frame in this.GetRawFrameKeys())
            {
                var value = this.Frames[frame];
                value.X = input.ReadValueS16() * 3.051851E-05f;
                value.Y = input.ReadValueS16() * 3.051851E-05f;
                value.Z = input.ReadValueS16() * 3.051851E-05f;

                var w2 = 1.0f - value.X * value.X - value.Y * value.Y - value.Z * value.Z;
                value.W = w2 <= 0.0f ? 0.0f : (float)Math.Sqrt(w2);
            }
        }

        public override Vector4 CalculateValue(float frame)
        {
            int start;
            int end;
            if (this.GetKey(frame, out start, out end))
            {
                var delta = frame - this.GetFrameKey(start);
                if (delta == 0.0f)
                {
                    return this.GetFrameValue(start);
                }

                var t = delta / (this.GetFrameKey(end) - this.GetFrameKey(start));
                return Quaternion3CompressedChannel.QuaternionSlerp(this.GetFrameValue(start), this.GetFrameValue(end), t);
            }

            return this.GetFrameValue(start);
        }
    }
}
