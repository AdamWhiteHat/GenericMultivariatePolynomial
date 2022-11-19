using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using ExtendedArithmetic.Internal;

namespace ExtendedArithmetic
{
	public static class GenericArithmetic<T>
	{
		public static T MinusOne;
		public static T Zero;
		public static T One;
		public static T Two;

		private static Dictionary<ExpressionType, Func<T, T, T>> _operationFunctionDictionary;

		private static Func<T, T, T> _powerTFunction = null;
		private static Func<T, int, T> _powerIntFunction = null;

		private static Func<T, T, bool> _lessthanFunction = null;
		private static Func<T, T, bool> _greaterthanFunction = null;
		private static Func<T, T, bool> _equalFunction = null;

		private static Func<T, T> _sqrtFunction = null;
		private static Func<T, T> _absFunction = null;
		private static Func<T, T> _truncateFunction = null;
		private static Func<T, int, T, T> _modpowFunction = null;
		private static Func<string, T> _parseFunction = null;
		private static Func<T, double, T> _logFunction = null;
		private static Func<T, byte[]> _tobytesFunction = null;
		private static MethodInfo _memberwiseCloneFunction = null;

		private static string _numberDecimalSeparator = null;

		static GenericArithmetic()
		{
			_operationFunctionDictionary = new Dictionary<ExpressionType, Func<T, T, T>>();
			MinusOne = GenericArithmetic<T>.Parse("-1");
			Zero = GenericArithmetic<T>.Parse("0");
			One = GenericArithmetic<T>.Parse("1");
			Two = GenericArithmetic<T>.Parse("2");
			_numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		}

		public static T Convert<TFrom>(TFrom value)
		{
			if (typeof(T) == typeof(Complex))
			{
				return (T)((object)new Complex((double)System.Convert.ChangeType(value, typeof(double)), 0d));
			}
			return ConvertImplementation<TFrom, T>.Convert(value);
		}

		public static T Add(T a, T b)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Add).Invoke(a, b);
		}

		public static T Subtract(T a, T b)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Subtract).Invoke(a, b);
		}

		public static T Multiply(T a, T b)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Multiply).Invoke(a, b);
		}

		public static T Divide(T a, T b)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Divide).Invoke(a, b);
		}

		public static T Modulo(T a, T b)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Modulo).Invoke(a, b);
		}

		public static T Power(T a, int b)
		{
			if (_powerIntFunction == null)
			{
				_powerIntFunction = GenericArithmeticFactory<T>.CreatePowerIntFunction();
			}

			if (_powerIntFunction != null)
			{
				return _powerIntFunction.Invoke(a, b);
			}
			else
			{
				return Power(a, ConvertImplementation<int, T>.Convert(b));
			}
		}

		public static T Power(T a, T b)
		{
			if (_powerTFunction == null)
			{
				_powerTFunction = GenericArithmeticFactory<T>.CreatePowerFunction();
			}
			return _powerTFunction.Invoke(a, b);
		}

		public static T Negate(T a)
		{
			return Multiply(a, MinusOne);
		}

		public static T Increment(T a)
		{
			return Add(a, One);
		}

		public static T Decrement(T a)
		{
			return Subtract(a, One);
		}

		public static bool GreaterThan(T a, T b)
		{
			if (IsComplexValueType(typeof(T)))
			{
				return ComplexEqualityOperator(a, b, ExpressionType.GreaterThan);
			}
			if (_greaterthanFunction == null)
			{
				_greaterthanFunction = GenericArithmeticFactory<T>.CreateGenericEqualityOperator(ExpressionType.GreaterThan);
			}
			return _greaterthanFunction.Invoke(a, b);
		}

		public static bool LessThan(T a, T b)
		{
			if (IsComplexValueType(typeof(T)))
			{
				return ComplexEqualityOperator(a, b, ExpressionType.LessThan);
			}
			if (_lessthanFunction == null)
			{
				_lessthanFunction = GenericArithmeticFactory<T>.CreateGenericEqualityOperator(ExpressionType.LessThan);
			}
			return _lessthanFunction.Invoke(a, b);
		}

		public static bool GreaterThanOrEqual(T a, T b)
		{
			return (GreaterThan(a, b) || Equal(a, b));
		}

		public static bool LessThanOrEqual(T a, T b)
		{
			return (LessThan(a, b) || Equal(a, b));
		}

		public static bool Equal(T a, T b)
		{
			if (_equalFunction == null)
			{
				_equalFunction = GenericArithmeticFactory<T>.CreateGenericEqualityOperator(ExpressionType.Equal);
			}

			if (a == null)
			{
				return (b == null);
			}
			return _equalFunction.Invoke(a, b);
		}

		public static bool NotEqual(T a, T b)
		{
			return !Equal(a, b);
		}

		public static T SquareRoot(T input)
		{
			Type typeFromHandle = typeof(T);
			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle) && (typeFromHandle != typeof(double)))
			{
				return (T)System.Convert.ChangeType(GenericArithmetic<double>.SquareRoot(System.Convert.ToDouble(input)), typeFromHandle);
			}

			if (_sqrtFunction == null)
			{
				_sqrtFunction = GenericArithmeticFactory<T>.CreateSqrtFunction();
			}
			return _sqrtFunction.Invoke(input);
		}

		public static T ModPow(T value, int exponent, T modulus)
		{
			if (_modpowFunction == null)
			{
				_modpowFunction = GenericArithmeticFactory<T>.CreateModPowFunction();
			}
			return _modpowFunction.Invoke(value, exponent, modulus);
		}

		public static T Truncate(T input)
		{
			if (_truncateFunction == null)
			{
				_truncateFunction = GenericArithmeticFactory<T>.CreateTruncateFunction();
			}
			return _truncateFunction.Invoke(input);
		}

		public static T Parse(string input)
		{
			if (_parseFunction == null)
			{
				_parseFunction = GenericArithmeticFactory<T>.CreateParseFunction();
			}
			return _parseFunction.Invoke(input);
		}

		public static T Max(T left, T right)
		{
			if (GreaterThanOrEqual(left, right))
			{
				return left;
			}
			return right;
		}

		public static T Abs(T input)
		{
			if (_absFunction == null)
			{
				_absFunction = GenericArithmeticFactory<T>.CreateAbsFunction();
			}
			return _absFunction.Invoke(input);
		}

		public static int Sign(T input)
		{
			if (GreaterThan(input, Zero))
			{
				return 1;
			}
			else if (LessThan(input, Zero))
			{
				return -1;
			}
			return 0;
		}

		public static T DivRem(T dividend, T divisor, out T remainder)
		{
			T rem = Modulo(dividend, divisor);
			remainder = rem;
			return Divide(dividend, divisor);
		}

		public static T Clone(T obj)
		{
			if (_memberwiseCloneFunction == null)
			{
				_memberwiseCloneFunction = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)_memberwiseCloneFunction.Invoke(obj, null);
		}

		public static T GCD(IEnumerable<T> array)
		{
			return array.Aggregate(GCD);
		}

		public static T GCD(T left, T right)
		{
			T absLeft = Abs(left);
			T absRight = Abs(right);
			while (NotEqual(absLeft, Zero) && NotEqual(absRight, Zero))
			{
				if (GreaterThan(absLeft, absRight))
				{
					absLeft = Modulo(absLeft, absRight);
				}
				else
				{
					absRight = Modulo(absRight, absLeft);
				}
			}
			return Max(absLeft, absRight);
		}

		public static T Log(T value, double baseValue)
		{
			Type typeFromHandle = typeof(T);
			if (_logFunction == null)
			{
				_logFunction = GenericArithmeticFactory<T>.CreateLogFunction();
			}
			return _logFunction.Invoke(value, baseValue);
		}

		public static byte[] ToBytes(T input)
		{
			Type typeFromHandle = typeof(T);
			if (GenericArithmeticCommon.IsArithmeticValueType(typeFromHandle))
			{
				if (_tobytesFunction == null)
				{
					_tobytesFunction = GenericArithmeticFactory<T>.CreateValueTypeToBytesFunction();
				}
				return _tobytesFunction.Invoke(input);
			}
			else
			{
				return GenericArithmeticFactory<T>.CreateToBytesFunction(input).Invoke();
			}
		}

		public static string ToString(T input)
		{
			string result = input.ToString();

			// If there is a decimal point present
			if (result.Contains(_numberDecimalSeparator))
			{
				result = result.TrimEnd('0'); // Trim all trailing zeros			
				if (result.EndsWith(_numberDecimalSeparator)) // If all we are left with is a decimal point
				{
					result = result.TrimEnd(_numberDecimalSeparator.ToCharArray()); // Then remove it
				}
			}
			return result;
		}

		public static bool IsWholeNumber(T value)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				typeFromHandle = typeFromHandle.GetGenericArguments()[0];
			}
			TypeCode typeCode = GenericArithmeticCommon.GetTypeCode(typeof(T));
			uint typeCodeValue = (uint)typeCode;

			if (typeFromHandle == typeof(BigInteger))
			{
				return true;
			}
			else if (typeCodeValue > 2 && typeCodeValue < 14) // Is between Boolean and Single
			{
				return true;
			}
			else if (typeCode == TypeCode.Double || typeCode == TypeCode.Decimal)
			{
				return Equal(Modulo(value, One), Zero);
			}
			else if (typeFromHandle == typeof(Complex))
			{
				Complex? complexNullable = value as Complex?;
				if (complexNullable.HasValue)
				{
					Complex complexValue = complexNullable.Value;
					return (complexValue.Imaginary == 0 && complexValue.Real % 1 == 0);
				}
			}
			//else if (type == typeof(BigRational)) { }
			//else if (type == typeof(BigComplex)) { }

			return false;
		}

		public static bool IsFractionalValue(T value)
		{
			return (GenericArithmeticCommon.IsArithmeticValueType(value.GetType()) && !IsWholeNumber(value));
		}


		public static bool IsComplexValueType(Type type)
		{
			return type.Name.Contains("Complex");
		}

		internal static T SquareRootInternal(T input)
		{
			if (Equal(input, Zero)) { return Zero; }

			T n = Zero;
			T p = Zero;
			T low = Zero;
			T high = Abs(input);

			while (GreaterThan(high, Increment(low)))
			{
				n = Divide(Add(high, low), Two);
				p = Multiply(n, n);

				if (LessThan(input, p)) { high = n; }
				else if (GreaterThan(input, p)) { low = n; }
				else { break; }
			}
			return Equals(input, p) ? n : low;
		}

		internal static T ModPowInternal(T value, int exponent, T modulus)
		{
			T power = Power(value, exponent);
			return Modulo(power, modulus);
		}

		private static bool ComplexEqualityOperator(T left, T right, ExpressionType operationType)
		{
			if (!IsComplexValueType(typeof(T)))
			{
				throw new Exception("T must be a Complex type.");
			}

			if (typeof(T) == typeof(Complex))
			{
				Complex? l = left as Complex?;
				Complex? r = right as Complex?;
				if (!l.HasValue || !r.HasValue)
				{
					throw new Exception("Could not cast parameters to type: Complex.");
				}

				double lft = Complex.Abs(l.Value);
				double rght = Complex.Abs(r.Value);

				if (Math.Sign(l.Value.Real) == -1)
				{
					lft = -lft;
				}
				if (Math.Sign(r.Value.Real) == -1)
				{
					rght = -rght;
				}

				if (operationType == ExpressionType.GreaterThan) { return (lft > rght); }
				else if (operationType == ExpressionType.LessThan) { return (lft < rght); }
				else
				{
					throw new NotSupportedException($"Not a comparison expression type: {Enum.GetName(typeof(ExpressionType), operationType)}.");
				}
			}

			PropertyInfo realProperty = typeof(T).GetProperty("Real", BindingFlags.Public | BindingFlags.Instance);
			Type realType = realProperty.PropertyType;

			object leftReal = realProperty.GetValue(left);
			object rightReal = realProperty.GetValue(right);

			int leftRealSign;
			int rightRealSign;

			MethodInfo signMethod = null;
			if (GenericArithmeticCommon.IsArithmeticValueType(realType))
			{
				signMethod = typeof(Math).GetMethod("Sign", BindingFlags.Static | BindingFlags.Public);

				leftRealSign = (int)signMethod.Invoke(null, new object[] { leftReal });
				rightRealSign = (int)signMethod.Invoke(null, new object[] { rightReal });
			}
			else
			{
				var signProperty = realType.GetProperty("Sign", BindingFlags.Public | BindingFlags.Instance);

				leftRealSign = (int)signProperty.GetValue(leftReal);
				rightRealSign = (int)signProperty.GetValue(rightReal);
			}

			Delegate absFunction = GenericArithmeticFactory<T>.CreateAbsFunction_UnlikeReturnType();

			object abs_left = absFunction.DynamicInvoke(left);
			object abs_right = absFunction.DynamicInvoke(right);

			Type absResultType = absFunction.Method.ReturnType;

			MethodInfo negateMethod = null;
			if (GenericArithmeticCommon.IsArithmeticValueType(absResultType))
			{
				ParameterExpression absValueParam = Expression.Parameter(absResultType, "value");

				Delegate negateDelegate = Expression.Lambda(Expression.Negate(Expression.Variable(absResultType)), absValueParam).Compile();
				negateMethod = negateDelegate.GetMethodInfo();

				if (leftRealSign == -1)
				{
					abs_left = negateMethod.Invoke(abs_left, null);
				}
				if (rightRealSign == -1)
				{
					abs_right = negateMethod.Invoke(abs_right, null);
				}
			}
			else
			{
				negateMethod = absResultType.GetMethod("Negate", BindingFlags.Public | BindingFlags.Static);

				if (leftRealSign == -1)
				{
					abs_left = negateMethod.Invoke(null, new object[] { abs_left });
				}
				if (rightRealSign == -1)
				{
					abs_right = negateMethod.Invoke(null, new object[] { abs_right });
				}
			}

			MethodInfo compareMethod = absResultType.GetMethod("Compare", BindingFlags.Static | BindingFlags.Public);

			Type compareReturnType = compareMethod.ReturnType;

			ParameterExpression leftParameter = Expression.Parameter(typeof(object), "left");
			ParameterExpression rightParameter = Expression.Parameter(typeof(object), "right");

			// Expression.TypeAs
			Expression lp = ConvertIfNeeded(leftParameter, absResultType);
			Expression rp = ConvertIfNeeded(rightParameter, absResultType);

			Expression methodCallExpression = Expression.Call(compareMethod, lp, rp);
			Expression methodCall_AutoConversion = ConvertIfNeeded(methodCallExpression, compareMethod.ReturnType);

			var compareLambda = Expression.Lambda(methodCall_AutoConversion, leftParameter, rightParameter).Compile();

			object invokeResult = compareLambda.DynamicInvoke(abs_left, abs_right);

			int compareResult = (int)invokeResult;
			if (operationType == ExpressionType.GreaterThan)
			{
				return compareResult > 0;
			}
			else if (operationType == ExpressionType.LessThan)
			{
				return compareResult < 0;
			}
			else
			{
				throw new NotSupportedException($"Not a comparison expression type: {Enum.GetName(typeof(ExpressionType), operationType)}.");
			}
		}

		public class ComplexComparer<T> : IComparer<T>
		{
			public int Compare(T l, T r)
			{
				Type parameterType = typeof(T);
				var paramMethods = parameterType.GetMethods(BindingFlags.Static | BindingFlags.Public);
				MethodInfo absMethod = paramMethods.Where(mi => mi.Name == "Abs").FirstOrDefault();

				Type returnType = absMethod.ReturnType;
				var returnMethods = returnType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
				MethodInfo compareToMethod = returnMethods.Where(mi => (mi.Name == "CompareTo") && (mi.GetParameters().FirstOrDefault()?.ParameterType == typeof(double))).FirstOrDefault();

				ParameterExpression leftParameter = Expression.Parameter(parameterType, "left");
				ParameterExpression rightParameter = Expression.Parameter(parameterType, "right");

				MethodCallExpression leftAbs = Expression.Call(absMethod, leftParameter);
				MethodCallExpression rightAbs = Expression.Call(absMethod, rightParameter);

				Func<T, double> leftAbsFunc = Expression.Lambda<Func<T, double>>(leftAbs, leftParameter).Compile();
				Func<T, double> rightAbsFunc = Expression.Lambda<Func<T, double>>(rightAbs, rightParameter).Compile();

				double leftAbsResult = leftAbsFunc.Invoke(l);
				double rightAbsResult = rightAbsFunc.Invoke(r);

				var leftAbsConstant = Expression.Constant(leftAbsResult);
				ParameterExpression rightAbsParameter = Expression.Parameter(returnType, "rightAbs");

				MethodCallExpression compareToExpression = Expression.Call(leftAbsConstant, compareToMethod, rightAbsParameter);
				Func<double, int> compareToFunc = Expression.Lambda<Func<double, int>>(compareToExpression, rightAbsParameter).Compile();

				int result = compareToFunc.Invoke(rightAbsResult);
				return result;
			}
		}

		private static class ConvertImplementation<TFrom, TTo>
		{
			private static Func<TFrom, TTo> _convertFunction = null;

			public static TTo Convert(TFrom value)
			{
				if (_convertFunction == null)
				{
					_convertFunction = CreateConvertFunction();
				}
				return _convertFunction.Invoke(value);
			}

			private static Func<TFrom, TTo> CreateConvertFunction()
			{
				ParameterExpression value = Expression.Parameter(typeof(TFrom), "value");
				Expression convert = Expression.Convert(value, typeof(TTo));
				Func<TFrom, TTo> result = Expression.Lambda<Func<TFrom, TTo>>(convert, value).Compile();
				return result;
			}
		}

		private static Expression ConvertIfNeeded(Expression valueExpression, Type targetType)
		{
			Type expressionType = null;
			if (valueExpression.NodeType == ExpressionType.Parameter)
			{
				expressionType = ((ParameterExpression)valueExpression).Type;
			}
			else if (valueExpression.NodeType == ExpressionType.Call)
			{
				expressionType = ((MethodCallExpression)valueExpression).Method.ReturnType;
			}

			if (expressionType != targetType)
			{
				return Expression.Convert(valueExpression, targetType);
			}
			return valueExpression;
		}
	}
}
