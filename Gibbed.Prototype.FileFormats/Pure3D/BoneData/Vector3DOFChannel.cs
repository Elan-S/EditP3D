using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121104)]
    public class Vector3DOFChannel : AnimationChannel
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
                output.WriteValueF32(value.X);
                output.WriteValueF32(value.Y);
                output.WriteValueF32(value.Z);
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
                value.X = input.ReadValueF32();
                value.Y = input.ReadValueF32();
                value.Z = input.ReadValueF32();
            }
        }

        public override Vector4 CalculateValue(float frame)
        {
            var value = new Vector4();
            int start;
            int end;
            if (this.GetKey(frame, out start, out end))
            {
                var delta = frame - this.GetFrameKey(start);
                if (delta == 0.0f)
                {
                    value = this.GetFrameValue(start);
                }
                else
                {
                    var startValue = this.GetFrameValue(start);
                    var endValue = this.GetFrameValue(end);
                    var t = delta / (this.GetFrameKey(end) - this.GetFrameKey(start));
                    value.X = startValue.X + (endValue.X - startValue.X) * t;
                    value.Y = startValue.Y + (endValue.Y - startValue.Y) * t;
                    value.Z = startValue.Z + (endValue.Z - startValue.Z) * t;
                }
            }
            else
            {
                value = this.GetFrameValue(start);
            }

            return value;
        }
    }
}
