/* Copyright (c) 2012 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    internal static class TexturePreview
    {
        private const uint DdsMagic = 0x20534444;
        private const uint Dxt1 = 0x31545844;
        private const uint Dxt3 = 0x33545844;
        private const uint Dxt5 = 0x35545844;

        public static Image Decode(TexturePNG png, TextureData data)
        {
            if (data == null || data.Data == null || data.Data.Length == 0)
            {
                return null;
            }

            try
            {
                using (var memory = new MemoryStream(data.Data, false))
                using (var image = Image.FromStream(memory))
                {
                    return new Bitmap(image);
                }
            }
            catch
            {
                return null;
            }
        }

        public static Image Decode(TextureDDS dds, TextureData data)
        {
            if (dds == null || data == null || data.Data == null || data.Data.Length == 0)
            {
                return null;
            }

            int width;
            int height;
            uint fourCc;
            int offset;
            if (TryReadDdsHeader(data.Data, out width, out height, out fourCc, out offset) == false)
            {
                width = checked((int)dds.Width);
                height = checked((int)dds.Height);
                fourCc = (uint)dds.Algorithm;
                offset = 0;
            }

            return DecodeDxt(data.Data, offset, width, height, fourCc);
        }

        public static void ExportDds(TextureDDS dds, TextureData data, Stream output)
        {
            if (dds == null || data == null || data.Data == null)
            {
                throw new InvalidOperationException();
            }

            int width;
            int height;
            uint fourCc;
            int offset;
            if (TryReadDdsHeader(data.Data, out width, out height, out fourCc, out offset) == true)
            {
                output.Write(data.Data, 0, data.Data.Length);
                return;
            }

            WriteDdsHeader(output, dds.Width, dds.Height, (uint)dds.Algorithm, dds.NumMipMaps);
            output.Write(data.Data, 0, data.Data.Length);
        }

        public static Image Decode(TextureData data)
        {
            if (data == null)
            {
                return null;
            }

            var dds = data.ParentNode as TextureDDS;
            if (dds != null)
            {
                return Decode(dds, data);
            }

            var png = data.ParentNode as TexturePNG;
            if (png != null)
            {
                return Decode(png, data);
            }

            return null;
        }

        public static bool Export(TextureData data, Stream output)
        {
            var dds = data == null ? null : data.ParentNode as TextureDDS;
            if (dds == null)
            {
                return false;
            }

            ExportDds(dds, data, output);
            return true;
        }

        private static bool TryReadDdsHeader(byte[] data, out int width, out int height, out uint fourCc, out int offset)
        {
            width = 0;
            height = 0;
            fourCc = 0;
            offset = 0;

            if (data == null || data.Length < 128 || BitConverter.ToUInt32(data, 0) != DdsMagic)
            {
                return false;
            }

            height = BitConverter.ToInt32(data, 12);
            width = BitConverter.ToInt32(data, 16);
            fourCc = BitConverter.ToUInt32(data, 84);
            offset = 128;
            return IsSupportedDxt(fourCc) == true && width > 0 && height > 0;
        }

        private static Bitmap DecodeDxt(byte[] data, int offset, int width, int height, uint fourCc)
        {
            if (data == null || IsSupportedDxt(fourCc) == false || width <= 0 || height <= 0)
            {
                return null;
            }

            try
            {
                var bitmap = new Bitmap(width, height);
                for (int by = 0; by < height; by += 4)
                {
                    for (int bx = 0; bx < width; bx += 4)
                    {
                        int blockSize = fourCc == Dxt1 ? 8 : 16;
                        if (offset + blockSize > data.Length)
                        {
                            return bitmap;
                        }

                        var alpha = DecodeDxtAlpha(data, offset, fourCc);
                        int colorOffset = fourCc == Dxt1 ? offset : offset + 8;
                        var colors = DecodeDxtColors(BitConverter.ToUInt16(data, colorOffset),
                                                     BitConverter.ToUInt16(data, colorOffset + 2),
                                                     fourCc == Dxt1);
                        uint bits = BitConverter.ToUInt32(data, colorOffset + 4);
                        offset += blockSize;

                        for (int py = 0; py < 4; py++)
                        {
                            for (int px = 0; px < 4; px++)
                            {
                                int code = (int)((bits >> (2 * (py * 4 + px))) & 3);
                                if (bx + px < width && by + py < height)
                                {
                                    int alphaIndex = py * 4 + px;
                                    bitmap.SetPixel(bx + px,
                                                    by + py,
                                                    Color.FromArgb(Math.Min(alpha[alphaIndex], colors[code].A), colors[code]));
                                }
                            }
                        }
                    }
                }

                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        private static Color[] DecodeDxtColors(ushort c0, ushort c1, bool allowTransparent)
        {
            Color a = Rgb565(c0), b = Rgb565(c1);
            if (allowTransparent == true && c0 <= c1)
            {
                return new[]
                {
                    a,
                    b,
                    Color.FromArgb((a.R + b.R) / 2, (a.G + b.G) / 2, (a.B + b.B) / 2),
                    Color.FromArgb(0, 0, 0, 0),
                };
            }

            return new[]
            {
                a,
                b,
                Color.FromArgb((2 * a.R + b.R) / 3, (2 * a.G + b.G) / 3, (2 * a.B + b.B) / 3),
                Color.FromArgb((a.R + 2 * b.R) / 3, (a.G + 2 * b.G) / 3, (a.B + 2 * b.B) / 3),
            };
        }

        private static int[] DecodeDxtAlpha(byte[] data, int offset, uint fourCc)
        {
            var alpha = Enumerable.Repeat(255, 16).ToArray();
            if (fourCc == Dxt3)
            {
                ulong bits = BitConverter.ToUInt64(data, offset);
                for (int i = 0; i < 16; i++)
                {
                    alpha[i] = (int)(((bits >> (i * 4)) & 0xF) * 17);
                }
            }
            else if (fourCc == Dxt5)
            {
                int a0 = data[offset];
                int a1 = data[offset + 1];
                var table = new int[8];
                table[0] = a0;
                table[1] = a1;
                if (a0 > a1)
                {
                    table[2] = (6 * a0 + a1) / 7;
                    table[3] = (5 * a0 + 2 * a1) / 7;
                    table[4] = (4 * a0 + 3 * a1) / 7;
                    table[5] = (3 * a0 + 4 * a1) / 7;
                    table[6] = (2 * a0 + 5 * a1) / 7;
                    table[7] = (a0 + 6 * a1) / 7;
                }
                else
                {
                    table[2] = (4 * a0 + a1) / 5;
                    table[3] = (3 * a0 + 2 * a1) / 5;
                    table[4] = (2 * a0 + 3 * a1) / 5;
                    table[5] = (a0 + 4 * a1) / 5;
                    table[6] = 0;
                    table[7] = 255;
                }

                ulong bits = 0;
                for (int i = 0; i < 6; i++)
                {
                    bits |= ((ulong)data[offset + 2 + i]) << (8 * i);
                }

                for (int i = 0; i < 16; i++)
                {
                    alpha[i] = table[(int)((bits >> (i * 3)) & 7)];
                }
            }

            return alpha;
        }

        private static Color Rgb565(ushort value)
        {
            int r = ((value >> 11) & 31) * 255 / 31;
            int g = ((value >> 5) & 63) * 255 / 63;
            int b = (value & 31) * 255 / 31;
            return Color.FromArgb(r, g, b);
        }

        private static bool IsSupportedDxt(uint fourCc)
        {
            return fourCc == Dxt1 || fourCc == Dxt3 || fourCc == Dxt5;
        }

        private static void WriteDdsHeader(Stream output, uint width, uint height, uint fourCc, uint mipMapCount)
        {
            if (width == 0 || height == 0 || IsSupportedDxt(fourCc) == false)
            {
                throw new InvalidOperationException();
            }

            uint flags = 0x00021007;
            uint caps = 0x00001000;
            if (mipMapCount > 1)
            {
                flags |= 0x00020000;
                caps |= 0x00400008;
            }

            output.WriteValueU32(DdsMagic);
            output.WriteValueU32(124);
            output.WriteValueU32(flags);
            output.WriteValueU32(height);
            output.WriteValueU32(width);
            output.WriteValueU32(CalculateLinearSize(width, height, fourCc));
            output.WriteValueU32(0);
            output.WriteValueU32(mipMapCount > 1 ? mipMapCount : 0);
            for (int i = 0; i < 11; i++)
            {
                output.WriteValueU32(0);
            }

            output.WriteValueU32(32);
            output.WriteValueU32(0x00000004);
            output.WriteValueU32(fourCc);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
            output.WriteValueU32(caps);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
            output.WriteValueU32(0);
        }

        private static uint CalculateLinearSize(uint width, uint height, uint fourCc)
        {
            uint blockSize = fourCc == Dxt1 ? 8u : 16u;
            return Math.Max(1u, (width + 3) / 4) * Math.Max(1u, (height + 3) / 4) * blockSize;
        }
    }
}
