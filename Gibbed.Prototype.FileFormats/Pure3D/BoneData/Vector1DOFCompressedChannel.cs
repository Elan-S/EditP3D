using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121102)]
    public class Vector1DOFCompressedChannel : AnimationChannel
    {
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
                output.WriteValueF32(this.Frames[frame].X);
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
                value.X = input.ReadValueF32();
            }
        }

        public override Vector4 CalculateValue(float frame)
        {
            var value = new Vector4
            {
                X = this.BaseValues.X,
                Y = this.BaseValues.Y,
                Z = this.BaseValues.Z,
            };

            int start;
            int end;
            if (this.GetKey(frame, out start, out end))
            {
                var delta = frame - this.GetFrameKey(start);
                if (delta == 0.0f)
                {
                    value[this.Mapping] = this.GetFrameValue(start).X;
                }
                else
                {
                    var startValue = this.GetFrameValue(start);
                    var endValue = this.GetFrameValue(end);
                    var span = this.GetFrameKey(end) - this.GetFrameKey(start);
                    value[this.Mapping] = (endValue.X - startValue.X) * (delta / span) + startValue.X;
                }
            }
            else
            {
                value[this.Mapping] = this.GetFrameValue(start).X;
            }

            return value;
        }
    }
}
