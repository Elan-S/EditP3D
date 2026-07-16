using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121100)]
    public class Vector1DOFChannel : AnimationChannel
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
                output.WriteValueF32(this.Frames[frame].X);
            }
        }

        public override void Deserialize(Stream input)
        {
            base.Deserialize(input);
            this.Mapping = 0;
            this.BaseValues = new Vector3();
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
                    value.X = this.GetFrameValue(start).X;
                }
                else
                {
                    var span = this.GetFrameKey(end) - this.GetFrameKey(start);
                    value.X = (this.GetFrameValue(end).X - this.GetFrameValue(start).X) * (delta / span) + this.GetFrameValue(start).X;
                }
            }
            else if (this.Frames != null && this.Frames.Count > 0)
            {
                value.X = this.GetFrameValue(start).X;
            }

            return value;
        }
    }
}
