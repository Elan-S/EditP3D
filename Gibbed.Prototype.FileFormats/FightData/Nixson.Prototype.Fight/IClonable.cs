using System;
using Nixson.Common;

namespace Nixson.Prototype.Fight
{
	internal interface IClonable
	{
		object Clone(PrototypeGame game);
	}
}
