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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Gibbed.IO;

namespace Gibbed.Prototype.FileFormats
{
    public class Pure3DFile
    {
        public Endian Endian;
        public Pure3D.Pure3DGame Game;
        public List<Pure3D.BaseNode> Nodes = new List<Pure3D.BaseNode>();

        private void SerializeNode(Stream output, Pure3D.BaseNode node)
        {
            Stream childrenStream = new MemoryStream();
            foreach (Pure3D.BaseNode child in node.Children)
            {
                this.SerializeNode(childrenStream, child);
            }

            Stream nodeStream = new MemoryStream();
            node.Serialize(nodeStream);

            output.WriteValueU32(node.TypeId);
            output.WriteValueU32((UInt32)(12 + nodeStream.Length));
            output.WriteValueU32((UInt32)(12 + nodeStream.Length + childrenStream.Length));

            nodeStream.Seek(0, SeekOrigin.Begin);
            output.WriteFromStream(nodeStream, nodeStream.Length);

            childrenStream.Seek(0, SeekOrigin.Begin);
            output.WriteFromStream(childrenStream, childrenStream.Length);
        }

        public void Serialize(Stream output)
        {
            Stream nodesStream = new MemoryStream();
            foreach (Pure3D.BaseNode node in this.Nodes)
            {
                this.SerializeNode(nodesStream, node);
            }

            output.WriteValueU32(0xFF443350);
            output.WriteValueU32(12);
            output.WriteValueU32((UInt32)(12 + nodesStream.Length));
            nodesStream.Seek(0, SeekOrigin.Begin);
            output.WriteFromStream(nodesStream, nodesStream.Length);
        }

        private static Pure3D.BaseNode DeserializeNode(Stream input, Endian endian, Pure3D.BaseNode parent, Pure3D.Pure3DGame game)
        {
            var start = input.Position;

            var typeId = input.ReadValueU32(endian);
            var headerSize = input.ReadValueU32(endian);
            var totalSize = input.ReadValueU32(endian);

            if (headerSize < 12 ||
                totalSize < headerSize ||
                start + headerSize > input.Length ||
                start + totalSize > input.Length)
            {
                throw new FormatException("invalid Pure3D chunk size");
            }

            var current = Pure3D.NodeFactory.CreateNode(typeId, game);
            
            if (current != null)
            {
                current.StartPosition = (uint)start;
                current.HeaderSize = headerSize;
                current.TotalSize = totalSize;

                try
                {
                    current.Deserialize(input);
                    if (input.Position != start + headerSize)
                    {
                        throw new FormatException("Pure3D chunk parser did not consume the header payload");
                    }
                }
                catch (Exception)
                {
                    input.Position = start + 12;
                    current = ReadUnknownNode(input, typeId, start, headerSize, totalSize);
                }
            }
            else
            {
                current = ReadUnknownNode(input, typeId, start, headerSize, totalSize);
            }

            current.ParentNode = parent;

            if (input.Position != start + headerSize)
            {
                throw new FormatException();
            }

            var end = start + totalSize;

            if (current is Pure3D.OpaqueNode)
            {
                input.Position = end;
                return current;
            }

            while (input.Position < end)
            {
                var child = DeserializeNode(input, endian, current, game);
                current.Children.Add(child);
            }

            if (input.Position != end)
            {
                throw new FormatException();
            }

            return current;
        }

        private static Pure3D.Unknown ReadUnknownNode(Stream input, uint typeId, long start, uint headerSize, uint totalSize)
        {
            var unknown = new Pure3D.Unknown(typeId);
            unknown.StartPosition = (uint)start;
            unknown.HeaderSize = headerSize;
            unknown.TotalSize = totalSize;
            unknown.Data = input.ReadBytes((int)headerSize - 12);
            return unknown;
        }

        public const uint Signature = 0xFF443350; // 'P3D\xFF'

        private static Pure3D.Pure3DGame DetectGame(Stream input, Endian endian, long start, long end)
        {
            var position = input.Position;
            try
            {
                if (endian == Endian.Big &&
                    HasSpiderMan4RootCandidate(input, endian, start, end) == true &&
                    HasSpiderMan4Chunk(input, endian, start, end, 3) == true)
                {
                    return Pure3D.Pure3DGame.SpiderMan4;
                }

                if (HasPrototype2RootChunk(input, endian, start, end) == true)
                {
                    return Pure3D.Pure3DGame.Prototype2;
                }

                return Pure3D.Pure3DGame.Prototype1;
            }
            finally
            {
                input.Position = position;
            }
        }

        private static bool HasSpiderMan4Chunk(Stream input, Endian endian, long start, long end, int maxDepth)
        {
            var position = start;
            while (position < end)
            {
                if (position + 12 > input.Length)
                {
                    return false;
                }

                input.Position = position;
                var typeId = input.ReadValueU32(endian);
                var headerSize = input.ReadValueU32(endian);
                var totalSize = input.ReadValueU32(endian);

                if (headerSize < 12 ||
                    totalSize < headerSize ||
                    position + totalSize > input.Length)
                {
                    return false;
                }

                if (IsSpiderMan4Marker(typeId) == true)
                {
                    return true;
                }

                if (maxDepth > 0 &&
                    HasSpiderMan4Chunk(input, endian, position + headerSize, position + totalSize, maxDepth - 1) == true)
                {
                    return true;
                }

                position += totalSize;
            }

            return false;
        }

        private static bool HasSpiderMan4RootCandidate(Stream input, Endian endian, long start, long end)
        {
            var position = start;
            while (position < end)
            {
                if (position + 12 > input.Length)
                {
                    return false;
                }

                input.Position = position;
                var typeId = input.ReadValueU32(endian);
                var headerSize = input.ReadValueU32(endian);
                var totalSize = input.ReadValueU32(endian);

                if (headerSize < 12 ||
                    totalSize < headerSize ||
                    position + totalSize > input.Length)
                {
                    return false;
                }

                if (IsSpiderMan4RootCandidate(typeId) == true)
                {
                    return true;
                }

                position += totalSize;
            }

            return false;
        }

        private static bool HasPrototype2RootChunk(Stream input, Endian endian, long start, long end)
        {
            var position = start;
            while (position < end)
            {
                if (position + 12 > input.Length)
                {
                    return false;
                }

                input.Position = position;
                var typeId = input.ReadValueU32(endian);
                var headerSize = input.ReadValueU32(endian);
                var totalSize = input.ReadValueU32(endian);

                if (headerSize < 12 ||
                    totalSize < headerSize ||
                    position + totalSize > input.Length)
                {
                    return false;
                }

                if (IsPrototype2Marker(typeId) == true)
                {
                    return true;
                }

                position += totalSize;
            }

            return false;
        }

        private static bool IsPrototype2Marker(uint typeId)
        {
            return typeId == 0x00025000 ||
                   typeId == 0x00025001 ||
                   typeId == 0x00025002 ||
                   typeId == 0x00025003;
        }

        private static bool IsSpiderMan4Marker(uint typeId)
        {
            return typeId == 0x00121008 ||
                   typeId == 0x00121009 ||
                   typeId == 0x00121010 ||
                   typeId == 0x02F00001;
        }

        private static bool IsSpiderMan4RootCandidate(uint typeId)
        {
            return typeId == 0x00010040 ||
                   typeId == 0x00010041 ||
                   typeId == 0x00010042 ||
                   typeId == 0x00010043 ||
                   typeId == 0x00025000 ||
                   typeId == 0x00025001 ||
                   typeId == 0x00025002 ||
                   typeId == 0x00025003;
        }

        public void Deserialize(Stream input)
        {
            this.Deserialize(input, null);
        }

        public void Deserialize(Stream input, Pure3D.Pure3DGame? gameOverride)
        {
            var start = input.Position;

            var magic = input.ReadValueU32(Endian.Little);
            if (magic != Signature &&
                magic.Swap() != Signature)
            {
                throw new FormatException("not a Pure3D file");
            }
            var endian = magic == Signature ? Endian.Little : Endian.Big;

            var headerSize = input.ReadValueU32(endian);
            if (headerSize != 12)
            {
                throw new FormatException("invalid header size");
            }

            var totalSize = input.ReadValueU32(endian);
            if (start + totalSize > input.Length)
            {
                throw new FormatException();
            }

            var end = start + totalSize;
            var game = gameOverride.HasValue == true ? gameOverride.Value : DetectGame(input, endian, input.Position, end);

            this.Nodes.Clear();
            while (input.Position < end)
            {
                this.Nodes.Add(DeserializeNode(input, endian, null, game));
            }

            if (input.Position != end)
            {
                throw new FormatException();
            }

            this.Endian = endian;
            this.Game = game;
        }
    }
}
