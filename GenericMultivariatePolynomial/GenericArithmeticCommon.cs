using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ExtendedArithmetic.Internal
{
	internal static class GenericArithmeticCommon
	{
		internal static bool IsArithmeticValueType(Type type)
		{
			TypeCode typeCode = GetTypeCode(type);
			if ((uint)(typeCode - 7) <= 8u)
			{
				return true;
			}

			return false;
		}

		internal static TypeCode GetTypeCode(Type fromType)
		{
			Type type = fromType;
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments()[0];
			}
			if (type.IsEnum)
			{
				return TypeCode.Object;
			}

			return Type.GetTypeCode(type);
		}
	}
}
