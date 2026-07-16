using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    [KnownType(0x0012110E)]
    public class VertexMorphChannel : AnimationChannel
    {
        public VertexMorphKey[] Keys { get; set; }

        [Browsable(false)]
        public byte[] ExtraData { get; set; }

        public override void Serialize(Stream output)
        {
            base.Serialize(output);
            output.WriteValueU32(this.NumberOfFrames);

            if (this.Keys != null)
            {
                foreach (var key in this.Keys)
                {
                    output.WriteValueU16(key.Frame);
                }

                foreach (var key in this.Keys)
                {
                    output.WriteValueU32(key.Value);
                }
            }

            if (this.ExtraData != null)
            {
                output.WriteBytes(this.ExtraData);
            }
        }

        public override void Deserialize(Stream input)
        {
            base.Deserialize(input);
            this.Mapping = 0;
            this.BaseValues = new Vector3();
            this.NumberOfFrames = input.ReadValueU32();

            var end = this.StartPosition + this.HeaderSize;
            var remaining = Math.Max(0, end - input.Position);
            var keyCount = remaining >= this.NumberOfFrames * 6
                               ? (int)this.NumberOfFrames
                               : 0;

            this.Keys = new VertexMorphKey[keyCount];
            this.Frames = new Dictionary<ushort, Vector4>();
            this.FrameOrder = new ushort[keyCount];

            for (int i = 0; i < keyCount; i++)
            {
                var frame = input.ReadValueU16();
                this.FrameOrder[i] = frame;
            }

            for (int i = 0; i < keyCount; i++)
            {
                var value = input.ReadValueU32();
                var frame = this.FrameOrder[i];
                this.Keys[i] = new VertexMorphKey(frame, value);
                this.Frames[frame] = new Vector4 { X = value };
            }

            remaining = Math.Max(0, end - input.Position);
            this.ExtraData = remaining > 0 ? input.ReadBytes((int)remaining) : new byte[0];
        }

        public override void ReadFrames(byte[] animationData)
        {
            // VRTX chunks seen so far store their key data inline.
        }

        protected override void ReadChannel(Stream input)
        {
        }

        public override Vector4 CalculateValue(float frame)
        {
            if (this.Keys == null || this.Keys.Length == 0)
            {
                return new Vector4();
            }

            var ordered = this.Keys.OrderBy(k => k.Frame).ToArray();
            if (ordered.Length == 1 || frame <= ordered[0].Frame)
            {
                return new Vector4 { X = ordered[0].Value };
            }

            for (int i = 0; i + 1 < ordered.Length; i++)
            {
                if (frame > ordered[i + 1].Frame)
                {
                    continue;
                }

                var span = ordered[i + 1].Frame - ordered[i].Frame;
                if (span <= 0)
                {
                    return new Vector4 { X = ordered[i + 1].Value };
                }

                var t = (frame - ordered[i].Frame) / span;
                return new Vector4
                {
                    X = ordered[i].Value + (ordered[i + 1].Value - ordered[i].Value) * t,
                };
            }

            return new Vector4 { X = ordered[ordered.Length - 1].Value };
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class VertexMorphKey
    {
        public VertexMorphKey(ushort frame, uint value)
        {
            this.Frame = frame;
            this.Value = value;
        }

        public ushort Frame { get; private set; }
        public uint Value { get; private set; }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}: {1}", this.Frame, this.Value);
        }
    }
}
