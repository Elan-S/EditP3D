using System;
using System.Collections.Generic;
using System.Reflection;
using Nixson.Common;

namespace Nixson.Prototype.Fight
{
	public abstract class Factory<TType, TAttribute> where TType : FightNode where TAttribute : KnownHashAttribute
	{
		private static bool IsLookupBuilt
		{
			get
			{
				return Factory<TType, TAttribute>._p1Lookup != null && Factory<TType, TAttribute>._p2Lookup != null;
			}
		}
		private static void BuildLookup()
		{
			Factory<TType, TAttribute>._p1Lookup = new Dictionary<ulong, Type>();
			Factory<TType, TAttribute>._p2Lookup = new Dictionary<ulong, Type>();
			foreach (Type type in Assembly.GetAssembly(typeof(TType)).GetTypes())
			{
				if (type.IsSubclassOf(typeof(TType)))
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(TAttribute), false);
					if (customAttributes.Length != 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							TAttribute tattribute = (TAttribute)((object)customAttributes[j]);
							if (type.Namespace.Contains(".Prototype1."))
							{
								Factory<TType, TAttribute>._p1Lookup.Add(tattribute.Hash, type);
							}
							else if (type.Namespace.Contains(".Prototype2."))
							{
								Factory<TType, TAttribute>._p2Lookup.Add(tattribute.Hash, type);
							}
							else
							{
								Factory<TType, TAttribute>._p1Lookup.Add(tattribute.Hash, type);
								Factory<TType, TAttribute>._p2Lookup.Add(tattribute.Hash, type);
							}
						}
					}
				}
			}
		}
		public static List<Type> GetTypes(PrototypeGame game)
		{
			if (!Factory<TType, TAttribute>.IsLookupBuilt)
			{
				Factory<TType, TAttribute>.BuildLookup();
			}
			return new List<Type>(((game == PrototypeGame.P1) ? Factory<TType, TAttribute>._p1Lookup : Factory<TType, TAttribute>._p2Lookup).Values);
		}
		public static Type GetType(PrototypeGame game, ulong typeId)
		{
			if (!Factory<TType, TAttribute>.IsLookupBuilt)
			{
				Factory<TType, TAttribute>.BuildLookup();
			}
			Dictionary<ulong, Type> dictionary = (game == PrototypeGame.P1) ? Factory<TType, TAttribute>._p1Lookup : Factory<TType, TAttribute>._p2Lookup;
			if (!dictionary.ContainsKey(typeId))
			{
				return null;
			}
			return dictionary[typeId];
		}
		public static TType Build(PrototypeGame game, ulong typeId)
		{
			if (!Factory<TType, TAttribute>.IsLookupBuilt)
			{
				Factory<TType, TAttribute>.BuildLookup();
			}
			Dictionary<ulong, Type> dictionary = (game == PrototypeGame.P1) ? Factory<TType, TAttribute>._p1Lookup : Factory<TType, TAttribute>._p2Lookup;
			if (!dictionary.ContainsKey(typeId))
			{
				return default(TType);
			}
			TType ttype;
			try
			{
				ttype = (TType)((object)Activator.CreateInstance(dictionary[typeId]));
				ttype.TypeHash = typeId;
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			return ttype;
		}
		private static Dictionary<ulong, Type> _p1Lookup;
		private static Dictionary<ulong, Type> _p2Lookup;
	}
}
