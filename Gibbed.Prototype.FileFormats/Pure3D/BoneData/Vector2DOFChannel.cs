using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121103)]
    public class Vector2DOFChannel : AnimationChannel
    {
        private static readonly int[][] StaticIndex =
        {
            new[] { 1, 2 },
            new[] { 0, 2 },
            new[] { 0, 1 },
        };

        public override void Serialize(Stream output)
        {
            base.Serialize(output);
            output.WriteValueU16(this.Mapping);
            this.BaseValues.Serialize(output);
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
                output.WriteValueF32(value.X);
                output.WriteValueF32(value.Y);
            }
        }

        public override void Deserialize(Stream input)
        {
            base.Deserialize(input);
            this.Mapping = input.ReadValueU16();
            this.BaseValues = new Vector3(input);
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
                var vector = new Vector2(input);
                value.X = vector.X;
                value.Y = vector.Y;
            }
        }

        public override Vector4 CalculateValue(float frame)
        {
            var value = new Vector4
            {
                X = this.BaseValues.X,
                Y = this.BaseValues.Y,
                Z = this.BaseValues.Z,
                W = 0.0f,
            };

            var indices = StaticIndex[this.Mapping];
            int start;
            int end;
            if (this.GetKey(frame, out start, out end))
            {
                var delta = frame - this.GetFrameKey(start);
                if (delta == 0.0f)
                {
                    value[indices[0]] = this.GetFrameValue(start).X;
                    value[indices[1]] = this.GetFrameValue(start).Y;
                }
                else
                {
                    var startValue = this.GetFrameValue(start);
                    var endValue = this.GetFrameValue(end);
                    var t = delta / (this.GetFrameKey(end) - this.GetFrameKey(start));
                    value[indices[0]] = startValue.X + (endValue.X - startValue.X) * t;
                    value[indices[1]] = startValue.Y + (endValue.Y - startValue.Y) * t;
                }
            }
            else
            {
                value[indices[0]] = this.GetFrameValue(start).X;
                value[indices[1]] = this.GetFrameValue(start).Y;
            }

            return value;
        }
    }
}
