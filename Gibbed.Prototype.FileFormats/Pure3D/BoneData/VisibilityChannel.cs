using System.Collections.Generic;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x00121108)]
    public class VisibilityChannel : AnimationChannel
    {
        public ushort InitialVisibility { get; set; }

        public override void Serialize(Stream output)
        {
            base.Serialize(output);
            output.WriteValueU16(this.InitialVisibility);
            output.WriteValueU32(this.NumberOfFrames);
            if (this.FrameOrder == null)
            {
                return;
            }

            foreach (var frame in this.FrameOrder)
            {
                output.WriteValueU16(frame);
            }
        }

        public override void Deserialize(Stream input)
        {
            base.Deserialize(input);
            this.InitialVisibility = input.ReadValueU16();
            this.NumberOfFrames = input.ReadValueU32();
            this.FrameOrder = new ushort[this.NumberOfFrames];
            this.Frames = new Dictionary<ushort, Vector4>();
            for (int i = 0; i < this.NumberOfFrames; i++)
            {
                var frame = input.ReadValueU16();
                this.FrameOrder[i] = frame;
                this.Frames[frame] = new Vector4 { X = this.InitialVisibility != 0 ? 1.0f : 0.0f };
            }
        }

        public override void ReadFrames(byte[] animationData)
        {
            // PVIS chunks seen so far store their frame toggle list inline.
        }

        protected override void ReadChannel(Stream input)
        {
        }

        public override Vector4 CalculateValue(float frame)
        {
            return new Vector4 { X = this.IsVisible(frame) ? 1.0f : 0.0f };
        }

        public bool IsVisible(float frame)
        {
            bool visible = this.InitialVisibility != 0;
            var frames = this.OrderedFrameKeys;
            if (frames == null)
            {
                return visible;
            }

            foreach (var key in frames)
            {
                if (frame < key)
                {
                    break;
                }

                visible = !visible;
            }

            return visible;
        }
    }
}
