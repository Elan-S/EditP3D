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
using System.Reflection;

namespace Gibbed.Prototype.FileFormats.Pure3D
{
    internal static class NodeFactory
    {
        private static Dictionary<uint, Type> _Lookup;
        private static Dictionary<uint, Type> _Prototype2Lookup;
        private static Dictionary<uint, Type> _SpiderMan4Lookup;

        private static void BuildLookup()
        {
            _Lookup = new Dictionary<uint, Type>();
            _Prototype2Lookup = new Dictionary<uint, Type>();
            _SpiderMan4Lookup = new Dictionary<uint, Type>();

            foreach (Type type in Assembly.GetAssembly(typeof(NodeFactory)).GetTypes())
            {
                if (type.IsSubclassOf(typeof(BaseNode)) == true)
                {
                    object[] attributes = type.GetCustomAttributes(typeof(KnownTypeAttribute), false);
                    if (attributes.Length > 0)
                    {
                        foreach (KnownTypeAttribute attribute in attributes)
                        {
                            Dictionary<uint, Type> lookup;
                            if (type.Namespace != null && type.Namespace.EndsWith(".Prototype2") == true)
                            {
                                lookup = _Prototype2Lookup;
                            }
                            else if (type.Namespace != null && type.Namespace.EndsWith(".SpiderMan4") == true)
                            {
                                lookup = _SpiderMan4Lookup;
                            }
                            else
                            {
                                lookup = _Lookup;
                            }

                            if (lookup.ContainsKey(attribute.Id) == false)
                            {
                                lookup.Add(attribute.Id, type);
                            }
                        }
                    }
                }
            }
        }

        public static BaseNode CreateNode(uint typeId, bool prototype2)
        {
            return CreateNode(typeId, prototype2 == true ? Pure3DGame.Prototype2 : Pure3DGame.Prototype1);
        }

        public static BaseNode CreateNode(uint typeId, Pure3DGame game)
        {
            if (typeId == 0x00121001)
            {
                return new Bone();
            }

            if (_Lookup == null)
            {
                BuildLookup();
            }

            Type type;
            if (game == Pure3DGame.SpiderMan4 &&
                _SpiderMan4Lookup != null &&
                _SpiderMan4Lookup.TryGetValue(typeId, out type) == true)
            {
                return CreateNode(type);
            }

            if (game == Pure3DGame.Prototype2 &&
                _Prototype2Lookup != null &&
                _Prototype2Lookup.TryGetValue(typeId, out type) == true)
            {
                return CreateNode(type);
            }

            if (_Lookup == null ||
                _Lookup.TryGetValue(typeId, out type) == false)
            {
                return null;
            }

            return CreateNode(type);
        }

        private static BaseNode CreateNode(Type type)
        {
            BaseNode node;
            try
            {
                node = (BaseNode)Activator.CreateInstance(type);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
            return node;
        }
    }
}
