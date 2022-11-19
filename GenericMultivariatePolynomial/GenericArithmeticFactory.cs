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
	public static class GenericArithmeticFactory<T>
	{
		public static readonly T MinusOne;
		public static readonly T Zero;
		public static readonly T One;
		public static readonly T Two;

		private static Dictionary<ExpressionType, Func<T, T, T>> _operationFunctionDictionary;
		private static Dictionary<string, Func<T, T>> _unaryFuncDictionary;

		private static string _numberDecimalSeparator = null;

		static GenericArithmeticFactory()
		{
			MinusOne = GenericArithmetic<T>.Parse("-1");
			Zero = GenericArithmetic<T>.Parse("0");
			One = GenericArithmetic<T>.Parse("1");
			Two = GenericArithmetic<T>.Parse("2");
			_numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			_operationFunctionDictionary = new Dictionary<ExpressionType, Func<T, T, T>>();
			_unaryFuncDictionary = new Dictionary<string, Func<T, T>>();
		}

		public static Func<T, T> CreateSqrtFunction()
		{
			if (_unaryFuncDictionary.ContainsKey(nameof(Math.Sqrt)))
			{
				return _unaryFuncDictionary[nameof(Math.Sqrt)];
			}

			MethodInfo method;
			Type typeFromHandle = typeof(T);
			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle))
			{
				method = typeof(Math).GetMethod("Sqrt", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				method = typeFromHandle.GetMethod("Sqrt", BindingFlags.Static | BindingFlags.Public);
			}

			if (method == null)
			{
				return GenericArithmetic<T>.SquareRootInternal;
			}

			ParameterExpression parameter = Expression.Parameter(typeFromHandle, "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			Func<T, T> result = Expression.Lambda<Func<T, T>>(methodCall, parameter).Compile();
			_unaryFuncDictionary.Add(nameof(Math.Sqrt), result);
			return result;
		}

		public static Func<T, T> CreateAbsFunction()
		{
			if (_unaryFuncDictionary.ContainsKey(nameof(Math.Abs)))
			{
				return _unaryFuncDictionary[nameof(Math.Abs)];
			}

			Type typeFromHandle = typeof(T);

			ParameterExpression value = Expression.Parameter(typeFromHandle, "value");
			MethodInfo method = null;
			Expression methodCall_AutoConversion = null;

			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle))
			{
				var methods = typeof(Math).GetMethods(BindingFlags.Static | BindingFlags.Public);
				var absMethods = methods.Where(mi => mi.Name == "Abs");
				absMethods = absMethods.Where(mi => mi.GetParameters()[0].ParameterType == typeFromHandle);
				method = absMethods.FirstOrDefault();

				if (method == null)
				{
					throw new NotSupportedException($"Cannot find public static method 'Abs' for type of {typeFromHandle.FullName}.");
				}

				methodCall_AutoConversion = Expression.Call(method, value);
			}
			else
			{
				var methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
				var absMethods = methods.Where(mi => mi.Name == "Abs").ToList();
				method = absMethods.FirstOrDefault();

				if (method == null)
				{
					throw new NotSupportedException($"Cannot find public static method 'Abs' for type of {typeFromHandle.FullName}.");
				}

				Expression methodCallExpression = Expression.Call(method, value);
				methodCall_AutoConversion = ConvertIfNeeded(methodCallExpression, typeFromHandle);
			}

			Func<T, T> result = Expression.Lambda<Func<T, T>>(methodCall_AutoConversion, value).Compile();
			_unaryFuncDictionary.Add(nameof(Math.Abs), result);
			return result;
		}

		public static Func<T, T> CreateTruncateFunction()
		{
			if (_unaryFuncDictionary.ContainsKey(nameof(Math.Truncate)))
			{
				return _unaryFuncDictionary[nameof(Math.Truncate)];
			}

			MethodInfo method = null;
			Type typeFromHandle = typeof(T);

			if (typeFromHandle == typeof(double) || typeFromHandle == typeof(decimal))
			{
				method = typeof(Math).GetMethod("Truncate", new Type[] { typeof(T) });
			}
			else
			{
				method = typeFromHandle.GetMethod("Truncate", new Type[] { typeof(T) });
			}

			if (method == null)
			{
				return new Func<T, T>((arg) => arg);
			}

			ParameterExpression parameter = Expression.Parameter(typeFromHandle, "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			Func<T, T> result = Expression.Lambda<Func<T, T>>(methodCall, parameter).Compile();
			_unaryFuncDictionary.Add(nameof(Math.Truncate), result);
			return result;
		}

		internal static Func<T, T> CreateGenericUnaryFunction(ExpressionType operationType)
		{
			if (_unaryFuncDictionary.ContainsKey(Enum.GetName(typeof(ExpressionType), operationType)))
			{
				return _unaryFuncDictionary[Enum.GetName(typeof(ExpressionType), operationType)];
			}

			ParameterExpression value = Expression.Parameter(typeof(T), "value");

			UnaryExpression operation = null;
			if (operationType == ExpressionType.Negate)
			{
				operation = Expression.Negate(value);
			}
			else
			{
				throw new NotSupportedException($"ExpressionType not supported: {Enum.GetName(typeof(ExpressionType), operationType)}.");
			}

			Func<T, T> result = Expression.Lambda<Func<T, T>>(operation, value).Compile();
			_unaryFuncDictionary.Add(Enum.GetName(typeof(ExpressionType), operationType), result);
			return result;
		}

		public static Func<T, T, T> CreateGenericBinaryFunction(ExpressionType operationType)
		{
			if (_operationFunctionDictionary.ContainsKey(operationType))
			{
				return _operationFunctionDictionary[operationType];
			}

			ParameterExpression left = Expression.Parameter(typeof(T), "left");
			ParameterExpression right = Expression.Parameter(typeof(T), "right");

			BinaryExpression operation = null;
			if (operationType == ExpressionType.Add)
			{
				operation = Expression.Add(left, right);
			}
			else if (operationType == ExpressionType.Subtract)
			{
				operation = Expression.Subtract(left, right);
			}
			else if (operationType == ExpressionType.Multiply)
			{
				operation = Expression.Multiply(left, right);
			}
			else if (operationType == ExpressionType.Divide)
			{
				operation = Expression.Divide(left, right);
			}
			else if (operationType == ExpressionType.Modulo)
			{
				operation = Expression.Modulo(left, right);
			}
			else if (operationType == ExpressionType.Power)
			{
				operation = Expression.Power(left, right);
			}
			else
			{
				throw new NotSupportedException($"ExpressionType not supported: {Enum.GetName(typeof(ExpressionType), operationType)}.");
			}

			Func<T, T, T> result = Expression.Lambda<Func<T, T, T>>(operation, left, right).Compile();
			_operationFunctionDictionary.Add(operationType, result);
			return result;
		}

		public static Func<T, T, T> CreatePowerFunction()
		{
			if (_operationFunctionDictionary.ContainsKey(ExpressionType.Power))
			{
				return _operationFunctionDictionary[ExpressionType.Power];
			}

			Type typeFromHandle = typeof(T);

			ParameterExpression baseVal = Expression.Parameter(typeFromHandle, "baseValue");
			ParameterExpression exponent = Expression.Parameter(typeFromHandle, "exponent");

			MethodInfo method = null;
			Expression methodCall_AutoConversion = null;


			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle))
			{
				method = typeof(Math).GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				var methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
				var powMethods = methods.Where(mi => mi.Name == "Pow").ToList();
				method = powMethods.FirstOrDefault();
			}

			if (method != null)
			{
				Expression exponent_AutoConversion = null;
				if (typeFromHandle == typeof(BigInteger))
				{
					exponent_AutoConversion = Expression.Convert(exponent, typeof(int), typeof(GenericArithmetic<T>).GetMethod("ConvertBigIntegerToInt", BindingFlags.Static | BindingFlags.NonPublic));

					methodCall_AutoConversion = Expression.Call(method, baseVal, exponent_AutoConversion);
				}
				else
				{
					Type returnType = method.ReturnType;

					ParameterInfo baseParameterInfo = method.GetParameters()[0];
					ParameterInfo expParameterInfo = method.GetParameters()[1];

					Expression baseVal_AutoConversion = ConvertIfNeeded(baseVal, baseParameterInfo.ParameterType);
					exponent_AutoConversion = ConvertIfNeeded(exponent, expParameterInfo.ParameterType);

					Expression methodCallExpression = Expression.Call(method, baseVal_AutoConversion, exponent_AutoConversion);

					methodCall_AutoConversion = ConvertIfNeeded(methodCallExpression, typeFromHandle);
				}
			}

			if (method == null || methodCall_AutoConversion == null)
			{
				throw new NotSupportedException($"Cannot find public static method 'Pow' for type of {typeFromHandle.FullName}.");
			}

			Func<T, T, T> result = Expression.Lambda<Func<T, T, T>>(methodCall_AutoConversion, baseVal, exponent).Compile();
			_operationFunctionDictionary.Add(ExpressionType.Power, result);
			return result;
		}

		public static Func<T, int, T> CreatePowerIntFunction()
		{
			Type typeFromHandle = typeof(T);

			ParameterExpression baseVal = Expression.Parameter(typeFromHandle, "baseValue");
			ParameterExpression exponent = Expression.Parameter(typeof(int), "exponent");

			MethodInfo method = null;
			Expression methodCall_AutoConversion = null;

			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle))
			{
				method = typeof(Math).GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				var methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
				var powMethods = methods.Where(mi => mi.Name == "Pow").ToList();

				var expAssignable = powMethods.Where(mi => mi.GetParameters()[1].ParameterType.IsAssignableFrom(typeof(int)));

				method = powMethods.FirstOrDefault();
			}

			if (method != null)
			{
				Type returnType = method.ReturnType;

				ParameterInfo baseParameterInfo = method.GetParameters()[0];
				ParameterInfo expParameterInfo = method.GetParameters()[1];

				Expression baseVal_AutoConversion = ConvertIfNeeded(baseVal, baseParameterInfo.ParameterType);
				Expression exponent_AutoConversion = ConvertIfNeeded(exponent, expParameterInfo.ParameterType);

				Expression methodCallExpression = Expression.Call(method, baseVal_AutoConversion, exponent_AutoConversion);

				methodCall_AutoConversion = ConvertIfNeeded(methodCallExpression, typeFromHandle);
			}

			if (method == null || methodCall_AutoConversion == null)
			{
				throw new NotSupportedException($"Cannot find public static method 'Pow' for type of {typeFromHandle.FullName} who's exponent is assignable from int.");
			}

			Func<T, int, T> result = Expression.Lambda<Func<T, int, T>>(methodCall_AutoConversion, baseVal, exponent).Compile();
			return result;
		}

		public static Func<T, T, bool> CreateGenericEqualityOperator(ExpressionType operationType)
		{
			ParameterExpression left = Expression.Parameter(typeof(T), "left");
			ParameterExpression right = Expression.Parameter(typeof(T), "right");

			BinaryExpression comparison = null;
			if (operationType == ExpressionType.GreaterThan)
			{
				comparison = Expression.GreaterThan(left, right);
			}
			else if (operationType == ExpressionType.LessThan)
			{
				comparison = Expression.LessThan(left, right);
			}
			else if (operationType == ExpressionType.Equal)
			{
				comparison = Expression.Equal(left, right);
			}
			Func<T, T, bool> result = Expression.Lambda<Func<T, T, bool>>(comparison, left, right).Compile();
			return result;
		}

		public static Func<T, int, T, T> CreateModPowFunction()
		{
			Type typeFromHandle = typeof(T);

			MethodInfo method = typeFromHandle.GetMethod("ModPower", BindingFlags.Static | BindingFlags.Public);
			if (method == null)
			{
				return GenericArithmetic<T>.ModPowInternal;
			}

			ParameterExpression val = Expression.Parameter(typeFromHandle, "value");
			ParameterExpression exp = Expression.Parameter(typeFromHandle, "exponent");
			ParameterExpression mod = Expression.Parameter(typeFromHandle, "modulus");
			MethodCallExpression methodCall = Expression.Call(method, val, exp, mod);
			Func<T, int, T, T> result = Expression.Lambda<Func<T, int, T, T>>(methodCall, val, exp, mod).Compile();
			return result;
		}

		public static Func<T, double, T> CreateLogFunction()
		{
			MethodInfo[] methods = null;

			Type typeFromHandle = typeof(T);
			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle))
			{
				methods = typeof(Math).GetMethods(BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
			}

			var filteredMethods = methods.Where(mi => mi.Name == "Log" && mi.GetParameters().Count() == 2);
			MethodInfo method = filteredMethods.FirstOrDefault();
			if (method == null)
			{
				throw new NotSupportedException($"No such method 'Log' on type {typeFromHandle.FullName}.");
			}

			ParameterExpression val = Expression.Parameter(typeFromHandle, "value");

			ParameterInfo valueParameterInfo = method.GetParameters()[0];
			Expression value_AutoConversion = ConvertIfNeeded(val, valueParameterInfo.ParameterType);

			ParameterExpression baseVal = Expression.Parameter(typeof(double), "baseValue");
			MethodCallExpression methodCallExpression = Expression.Call(method, value_AutoConversion, baseVal);

			Expression methodCall_AutoConversion = ConvertIfNeeded(methodCallExpression, typeFromHandle);

			Func<T, double, T> result = Expression.Lambda<Func<T, double, T>>(methodCall_AutoConversion, val, baseVal).Compile();
			return result;
		}

		public static Func<string, T> CreateParseFunction()
		{
			Type typeFromHandle = typeof(T);

			MethodInfo[] methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
			var filteredMethods =
				methods.Where(
					mi => mi.Name == "Parse"
					&& mi.GetParameters().Count() == 1
					&& mi.GetParameters().First().ParameterType == typeof(string)
				);

			MethodInfo method = null;
			if (typeFromHandle == typeof(Complex))
			{
				method = typeof(HelperMethods).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				method = filteredMethods.FirstOrDefault();
			}

			if (method == null)
			{

				throw new NotSupportedException($"Cannot find public static method 'Parse' for type of {typeFromHandle.FullName}.");
			}

			ParameterExpression parameter = Expression.Parameter(typeof(string), "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			Func<string, T> result = Expression.Lambda<Func<string, T>>(methodCall, parameter).Compile();
			return result;
		}

		internal static Func<T, byte[]> CreateValueTypeToBytesFunction()
		{
			Type typeFromHandle = typeof(T);
			var allMethods = typeof(BitConverter).GetMethods(BindingFlags.Static | BindingFlags.Public);
			var matchingNameMethods = allMethods.Where(mi => mi.Name == "GetBytes");
			var matchingTypeMethods = matchingNameMethods.Where(mi => mi.GetParameters()[0].ParameterType == typeFromHandle);
			MethodInfo method = matchingTypeMethods.FirstOrDefault();
			ParameterExpression parameter = Expression.Parameter(typeFromHandle, "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			return Expression.Lambda<Func<T, byte[]>>(methodCall, parameter).Compile();
		}

		internal static Func<byte[]> CreateToBytesFunction(T instanceObject)
		{
			Type typeFromHandle = typeof(T);
			var allMethods = typeFromHandle.GetMethods(BindingFlags.Public | BindingFlags.Instance);
			var matchingMethods = allMethods.Where(mi => mi.Name == "ToByteArray");

			MethodInfo method = matchingMethods.FirstOrDefault();
			if (method == null)
			{
				throw new NotSupportedException($"Cannot find suitable method to convert instance of type {typeFromHandle.FullName} to an array of bytes.");
			}

			MethodCallExpression methodCall = Expression<T>.Call(Expression.Constant(instanceObject), method);
			return Expression.Lambda<Func<byte[]>>(methodCall).Compile();
		}

		internal static Delegate CreateAbsFunction_UnlikeReturnType()
		{
			Type typeFromHandle = typeof(T);
			var methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
			var absMethods = methods.Where(mi => mi.Name == "Abs").ToList();
			MethodInfo method = absMethods.FirstOrDefault();

			if (method == null)
			{
				throw new NotSupportedException($"Cannot find public static method 'Abs' for type of {typeFromHandle.FullName}.");
			}

			ParameterExpression parameter = Expression.Parameter(typeFromHandle, "value");
			Expression methodCallExpression = Expression.Call(method, parameter);
			Expression methodCall_AutoConversion = ConvertIfNeeded(methodCallExpression, method.ReturnType);

			Delegate result = Expression.Lambda(methodCall_AutoConversion, parameter).Compile();
			return result;
		}

		internal static Expression ConvertIfNeeded(Expression valueExpression, Type targetType)
		{
			Type returnType = null;
			if (valueExpression.NodeType == ExpressionType.Parameter)
			{
				returnType = ((ParameterExpression)valueExpression).Type;
			}
			else if (valueExpression.NodeType == ExpressionType.Call)
			{
				returnType = ((MethodCallExpression)valueExpression).Method.ReturnType;
			}

			if (returnType != targetType)
			{
				return Expression.Convert(valueExpression, targetType);
			}
			return valueExpression;
		}
	}
}
