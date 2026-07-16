using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121114)]
    public class Quaternion3CompressedChannel : AnimationChannel
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
                output.WriteByte((byte)(sbyte)(value.X * 127.0f));
                output.WriteByte((byte)(sbyte)(value.Y * 127.0f));
                output.WriteByte((byte)(sbyte)(value.Z * 127.0f));
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
                value.X = (sbyte)input.ReadValueU8() * 0.007874016f;
                value.Y = (sbyte)input.ReadValueU8() * 0.007874016f;
                value.Z = (sbyte)input.ReadValueU8() * 0.007874016f;

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
                return QuaternionSlerp(this.GetFrameValue(start), this.GetFrameValue(end), t);
            }

            return this.GetFrameValue(start);
        }

        internal static Vector4 QuaternionSlerp(Vector4 a, Vector4 b, float t)
        {
            var dot = a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
            if (dot < 0.0f)
            {
                b = new Vector4
                {
                    W = -b.W,
                    X = -b.X,
                    Y = -b.Y,
                    Z = -b.Z,
                };
                dot = -dot;
            }

            if (dot > 0.9995f)
            {
                return Normalize(new Vector4
                {
                    W = a.W + (b.W - a.W) * t,
                    X = a.X + (b.X - a.X) * t,
                    Y = a.Y + (b.Y - a.Y) * t,
                    Z = a.Z + (b.Z - a.Z) * t,
                });
            }

            dot = Math.Max(-1.0f, Math.Min(1.0f, dot));
            var theta0 = (float)Math.Acos(dot);
            var theta = theta0 * t;
            var sinTheta = (float)Math.Sin(theta);
            var sinTheta0 = (float)Math.Sin(theta0);
            var s0 = (float)Math.Cos(theta) - dot * sinTheta / sinTheta0;
            var s1 = sinTheta / sinTheta0;

            return Normalize(new Vector4
            {
                W = a.W * s0 + b.W * s1,
                X = a.X * s0 + b.X * s1,
                Y = a.Y * s0 + b.Y * s1,
                Z = a.Z * s0 + b.Z * s1,
            });
        }

        private static Vector4 Normalize(Vector4 value)
        {
            var length = (float)Math.Sqrt(value.W * value.W + value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            if (length <= 0.000001f)
            {
                return new Vector4 { W = 1.0f };
            }

            return new Vector4
            {
                W = value.W / length,
                X = value.X / length,
                Y = value.Y / length,
                Z = value.Z / length,
            };
        }
    }
}
