using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    public abstract class AnimationChannel : BaseNode
    {
        public uint Version { get; set; }
        public string TranslationType { get; set; }
        public uint NumberOfFrames { get; set; }
        public ushort Mapping { get; set; }
        [Browsable(false)]
        public Dictionary<ushort, Vector4> Frames { get; set; }
        public Vector3 BaseValues { get; set; }
        [Browsable(false)]
        public ushort[] FrameOrder { get; set; }

        public AnimationChannelFrame[] FrameValues
        {
            get
            {
                if (this.Frames == null)
                {
                    return new AnimationChannelFrame[0];
                }

                var keys = this.GetOrderedFrameKeys();
                return keys.Where(k => this.Frames.ContainsKey(k))
                           .Select(k => new AnimationChannelFrame(k, this.Frames[k]))
                           .ToArray();
            }
        }

        [Browsable(false)]
        public ushort[] OrderedFrameKeys
        {
            get { return this.GetOrderedFrameKeys(); }
        }

        public bool ContainsAnimData
        {
            get { return this.NumberOfFrames > 0; }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.TranslationType)
                       ? base.ToString()
                       : base.ToString() + " (" + this.TranslationType.TrimEnd('\0') + ")";
        }

        public override void Serialize(Stream output)
        {
            output.WriteValueU32(this.Version);
            output.WriteString(this.TranslationType);
        }

        public override void Deserialize(Stream input)
        {
            this.Version = input.ReadValueU32();
            this.TranslationType = input.ReadString(4);
        }

        public virtual void ReadFrames(byte[] animationData)
        {
            this.ReadFrames(animationData == null ? null : new[] { animationData });
        }

        public virtual void ReadFrames(IList<byte[]> animationDataBlocks)
        {
            if (this.ContainsAnimData == true || animationDataBlocks == null || animationDataBlocks.Count == 0)
            {
                return;
            }

            var reference = this.Children.OfType<AnimationDataReference>().FirstOrDefault();
            var animationData = this.GetReferencedAnimationData(animationDataBlocks, reference);
            if (reference == null || animationData == null || reference.Offset >= animationData.Length)
            {
                return;
            }

            using (var input = new MemoryStream(animationData))
            {
                input.Position = reference.Offset;
                this.NumberOfFrames = reference.Frames;
                this.Frames = new Dictionary<ushort, Vector4>();
                this.FrameOrder = new ushort[this.NumberOfFrames];
                for (int i = 0; i < this.NumberOfFrames; i++)
                {
                    var frame = input.ReadValueU16();
                    this.FrameOrder[i] = frame;
                    this.AddFrame(frame);
                }

                if (this.NumberOfFrames % 2 != 0)
                {
                    input.ReadBytes(2);
                }

                this.ReadChannel(input);
            }
        }

        public static List<byte[]> BuildAnimationDataBlocks(IEnumerable<AnimationChannel> channels, byte[] animationData)
        {
            return BuildAnimationDataBlocks(channels, animationData == null ? null : new[] { animationData });
        }

        public static List<byte[]> BuildAnimationDataBlocks(IEnumerable<AnimationChannel> channels, IList<byte[]> animationDataBlocks)
        {
            if (animationDataBlocks == null || animationDataBlocks.Count == 0)
            {
                return new List<byte[]>();
            }

            var blocks = animationDataBlocks.Where(b => b != null && b.Length > 0).ToList();
            if (blocks.Count != 1)
            {
                return blocks;
            }

            var references = (channels ?? Enumerable.Empty<AnimationChannel>())
                .Select(c => new
                {
                    Channel = c,
                    Reference = c == null ? null : c.Children.OfType<AnimationDataReference>().FirstOrDefault(),
                })
                .Where(r => r.Reference != null)
                .ToList();
            if (references.Count == 0)
            {
                return blocks;
            }

            int maxSegment = references.Max(r => GetReferenceSegment(r.Reference));
            if (maxSegment <= 0)
            {
                return blocks;
            }

            var segmentSizes = new int[maxSegment + 1];
            foreach (var item in references)
            {
                int segment = GetReferenceSegment(item.Reference);
                if (segment < 0 || segment >= segmentSizes.Length)
                {
                    continue;
                }

                long end = (long)item.Reference.Offset + item.Channel.GetExternalDataSize(item.Reference.Frames);
                if (end > segmentSizes[segment])
                {
                    segmentSizes[segment] = end > int.MaxValue ? int.MaxValue : (int)end;
                }
            }

            var source = blocks[0];
            var result = new List<byte[]>();
            int sourceOffset = 0;
            for (int i = 0; i < segmentSizes.Length && sourceOffset < source.Length; i++)
            {
                int length = AlignLength(segmentSizes[i], 16);
                length = Math.Max(0, Math.Min(length, source.Length - sourceOffset));
                var segmentData = new byte[length];
                Buffer.BlockCopy(source, sourceOffset, segmentData, 0, length);
                result.Add(segmentData);
                sourceOffset += length;
            }

            return result.Count == 0 ? blocks : result;
        }

        private byte[] GetReferencedAnimationData(IList<byte[]> animationDataBlocks, AnimationDataReference reference)
        {
            if (reference == null || animationDataBlocks == null || animationDataBlocks.Count == 0)
            {
                return null;
            }

            int index = 0;
            if (reference.ExtraData != null && reference.ExtraData.Length > 0)
            {
                index = reference.ExtraData[0];
            }

            if (index < 0 || index >= animationDataBlocks.Count)
            {
                index = 0;
            }

            return animationDataBlocks[index];
        }

        private static int GetReferenceSegment(AnimationDataReference reference)
        {
            return reference != null && reference.ExtraData != null && reference.ExtraData.Length > 0
                       ? reference.ExtraData[0]
                       : 0;
        }

        protected virtual int GetFrameValueSize()
        {
            if (this is Quaternion6CompressedChannel)
            {
                return 6;
            }

            if (this is Quaternion3CompressedChannel)
            {
                return 3;
            }

            if (this is Vector3DOFChannel)
            {
                return 12;
            }

            if (this is Vector3DOFCompressedChannel)
            {
                return 6;
            }

            if (this is Vector2DOFChannel)
            {
                return 8;
            }

            if (this is Vector2DOFCompressedChannel)
            {
                return 4;
            }

            if (this is Vector1DOFChannel || this is Vector1DOFCompressedChannel)
            {
                return 4;
            }

            return 0;
        }

        private int GetExternalDataSize(uint frameCount)
        {
            long frames = frameCount > int.MaxValue ? int.MaxValue : (int)frameCount;
            long frameTableSize = frames * 2;
            if ((frames & 1) != 0)
            {
                frameTableSize += 2;
            }

            long size = frameTableSize + (frames * this.GetFrameValueSize());
            return size > int.MaxValue ? int.MaxValue : AlignLength((int)size, 4);
        }

        private static int AlignLength(int value, int alignment)
        {
            return (value + alignment - 1) & ~(alignment - 1);
        }

        protected bool GetKey(float frame, out int start, out int end)
        {
            if (this.Frames == null || this.Frames.Count == 0)
            {
                start = end = 0;
                return false;
            }

            var keys = this.GetOrderedFrameKeys();
            if (keys.Length == 0)
            {
                start = end = 0;
                return false;
            }

            if (frame < keys[0])
            {
                start = end = 0;
                return false;
            }

            if (frame >= keys[keys.Length - 1])
            {
                start = end = keys.Length - 1;
                return false;
            }

            int low = 0;
            int high = keys.Length - 2;
            while (low <= high)
            {
                var current = low + ((high - low) / 2);
                if (frame < keys[current])
                {
                    high = current - 1;
                }
                else if (frame >= keys[current + 1])
                {
                    low = current + 1;
                }
                else
                {
                    start = current;
                    end = current + 1;
                    return true;
                }
            }

            start = Math.Max(0, Math.Min(keys.Length - 1, low));
            end = start;
            return false;
        }

        protected ushort[] GetOrderedFrameKeys()
        {
            var keys = this.GetRawFrameKeys();
            if (keys.Length < 2)
            {
                return keys;
            }

            var seen = new HashSet<ushort>();
            var uniqueKeys = new List<ushort>();
            foreach (var key in keys)
            {
                if (seen.Add(key) == true)
                {
                    uniqueKeys.Add(key);
                }
            }

            return uniqueKeys.OrderBy(k => k).ToArray();
        }

        protected ushort[] GetRawFrameKeys()
        {
            if (this.FrameOrder != null && this.FrameOrder.Length > 0)
            {
                return this.FrameOrder;
            }

            return this.Frames == null ? new ushort[0] : this.Frames.Keys.OrderBy(k => k).ToArray();
        }

        protected void AddFrame(ushort frame)
        {
            if (this.Frames.ContainsKey(frame) == false)
            {
                this.Frames.Add(frame, new Vector4());
            }
        }

        protected Vector4 GetFrameValue(int index)
        {
            var keys = this.GetOrderedFrameKeys();
            return this.Frames[keys[index]];
        }

        protected ushort GetFrameKey(int index)
        {
            return this.GetOrderedFrameKeys()[index];
        }

        protected abstract void ReadChannel(Stream input);

        public abstract Vector4 CalculateValue(float frame);

        protected static float ReadHalf(Stream input)
        {
            return HalfToSingle(input.ReadValueU16());
        }

        protected static void WriteHalf(Stream output, float value)
        {
            output.WriteValueU16(SingleToHalf(value));
        }

        private static float HalfToSingle(ushort value)
        {
            var sign = (value >> 15) & 1;
            var exponent = (value >> 10) & 31;
            var mantissa = value & 1023;

            if (exponent == 0)
            {
                return mantissa == 0
                           ? (sign == 0 ? 0.0f : -0.0f)
                           : (float)((sign == 0 ? 1.0 : -1.0) * Math.Pow(2.0, -14.0) * (mantissa / 1024.0));
            }

            if (exponent == 31)
            {
                return mantissa == 0
                           ? (sign == 0 ? float.PositiveInfinity : float.NegativeInfinity)
                           : float.NaN;
            }

            return (float)((sign == 0 ? 1.0 : -1.0) * Math.Pow(2.0, exponent - 15.0) * (1.0 + mantissa / 1024.0));
        }

        private static ushort SingleToHalf(float value)
        {
            var bits = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            var sign = (bits >> 16) & 0x8000;
            var exponent = (int)((bits >> 23) & 0xFF) - 127 + 15;
            var mantissa = bits & 0x007FFFFF;

            if (exponent <= 0)
            {
                if (exponent < -10)
                {
                    return (ushort)sign;
                }

                mantissa = (mantissa | 0x00800000) >> (1 - exponent);
                return (ushort)(sign | ((mantissa + 0x00001000) >> 13));
            }

            if (exponent >= 31)
            {
                return (ushort)(sign | 0x7C00);
            }

            return (ushort)(sign | ((uint)exponent << 10) | ((mantissa + 0x00001000) >> 13));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class AnimationChannelFrame
    {
        public AnimationChannelFrame(ushort frame, Vector4 value)
        {
            this.Frame = frame;
            this.Value = value;
        }

        public ushort Frame { get; private set; }
        public Vector4 Value { get; private set; }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                 "{0}: X={1:0.000000}, Y={2:0.000000}, Z={3:0.000000}, W={4:0.000000}",
                                 this.Frame,
                                 this.Value == null ? 0.0f : this.Value.X,
                                 this.Value == null ? 0.0f : this.Value.Y,
                                 this.Value == null ? 0.0f : this.Value.Z,
                                 this.Value == null ? 0.0f : this.Value.W);
        }
    }
}
