using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121119)]
    public class Vector3DOFCompressedChannel : AnimationChannel
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
                WriteHalf(output, value.X);
                WriteHalf(output, value.Y);
                WriteHalf(output, value.Z);
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
                value.X = ReadHalf(input);
                value.Y = ReadHalf(input);
                value.Z = ReadHalf(input);
            }
        }

        public override Vector4 CalculateValue(float frame)
        {
            var value = new Vector4();
            int start;
            int end;
            if (this.GetKey(frame, out start, out end))
            {
                var startValue = this.GetFrameValue(start);
                var endValue = this.GetFrameValue(end);
                var delta = frame - this.GetFrameKey(start);
                if (delta == 0.0f)
                {
                    value = startValue;
                }
                else
                {
                    var t = delta / (this.GetFrameKey(end) - this.GetFrameKey(start));
                    value.X = (endValue.X - startValue.X) * t + startValue.X;
                    value.Y = (endValue.Y - startValue.Y) * t + startValue.Y;
                    value.Z = (endValue.Z - startValue.Z) * t + startValue.Z;
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
