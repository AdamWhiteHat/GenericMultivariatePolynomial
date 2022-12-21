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
		/// <summary>Gets a value that represents the number negative one (-1).</summary>
		public static T MinusOne;
		/// <summary>Gets a value that represents the number zero (0).</summary>
		public static T Zero;
		/// <summary>Gets a value that represents the number one (1).</summary>
		public static T One;
		/// <summary>Gets a value that represents the number two (2).</summary>
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

		/// <summary>
		/// Adds two values and returns the result.
		/// </summary>
		/// <param name="augend">The first value to add.</param>
		/// <param name="addend">The second value to add.</param>
		/// <returns>The sum of the augend and addend.</returns>
		public static T Add(T augend, T addend)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Add).Invoke(augend, addend);
		}

		/// <summary>
		/// Subtracts one value from another and returns the result.
		/// </summary>
		/// <param name="minuend">The value to subtract from.</param>
		/// <param name="subtrahend">The value to subtract.</param>
		/// <returns>The difference of the minuend and the subtrahend.</returns>
		public static T Subtract(T minuend, T subtrahend)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Subtract).Invoke(minuend, subtrahend);
		}

		/// <summary>
		/// Returns the product of two values.
		/// </summary>
		/// <param name="multiplicand">The first number to multiply.</param>
		/// <param name="multiplier">The second number to multiply.</param>
		/// <returns>The product of the multiplicand and the multiplier.</returns>
		public static T Multiply(T multiplicand, T multiplier)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Multiply).Invoke(multiplicand, multiplier);
		}

		/// <summary>
		/// Returns the quotient of two values.
		/// </summary>
		/// <param name="dividend">The number that is being divided.</param>
		/// <param name="divisor">The number by which to divide.</param>
		/// <returns>The quotient of the dividend divided by the divisor.</returns>
		public static T Divide(T dividend, T divisor)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Divide).Invoke(dividend, divisor);
		}

		/// <summary>
		/// Returns the remainder of a dividend divided by a modulus.
		/// </summary>
		/// <param name="dividend">The number that is being divided.</param>
		/// <param name="modulus">The number by which to divide.</param>
		/// <returns>The remainder of the dividend divided by the modulus.</returns>
		public static T Modulo(T dividend, T modulus)
		{
			return GenericArithmeticFactory<T>.CreateGenericBinaryFunction(ExpressionType.Modulo).Invoke(dividend, modulus);
		}

		/// <summary>
		/// Raises a base number to an exponent and returns the result.
		/// </summary>
		/// <param name="base">The number to raise to the exponent power.</param>
		/// <param name="exponent">The exponent to raise the base by.</param>
		/// <returns>The result of raising the base to the exponent.</returns>
		public static T Power(T @base, int exponent)
		{
			if (_powerIntFunction == null)
			{
				_powerIntFunction = GenericArithmeticFactory<T>.CreatePowerIntFunction();
			}

			if (_powerIntFunction != null)
			{
				return _powerIntFunction.Invoke(@base, exponent);
			}
			else
			{
				return Power(@base, ConvertImplementation<int, T>.Convert(exponent));
			}
		}

		/// <summary>
		/// Raises a base number to an exponent and returns the result.
		/// </summary>
		/// <param name="base">The number to raise to the exponent power.</param>
		/// <param name="exponent">The exponent to raise the base by.</param>
		/// <returns>The result of raising the base to the exponent.</returns>
		public static T Power(T @base, T exponent)
		{
			if (_powerTFunction == null)
			{
				_powerTFunction = GenericArithmeticFactory<T>.CreatePowerFunction();
			}
			return _powerTFunction.Invoke(@base, exponent);
		}

		public static T Negate(T value)
		{
			return Multiply(value, MinusOne);
		}

		public static T Increment(T value)
		{
			return Add(value, One);
		}

		public static T Decrement(T value)
		{
			return Subtract(value, One);
		}

		public static bool GreaterThan(T left, T right)
		{
			if (ComplexHelperMethods.IsComplexValueType(typeof(T)))
			{
				return ComplexHelperMethods.ComplexEqualityOperator(left, right, ExpressionType.GreaterThan);
			}
			if (_greaterthanFunction == null)
			{
				_greaterthanFunction = GenericArithmeticFactory<T>.CreateGenericEqualityOperator(ExpressionType.GreaterThan);
			}
			return _greaterthanFunction.Invoke(left, right);
		}

		public static bool LessThan(T left, T right)
		{
			if (ComplexHelperMethods.IsComplexValueType(typeof(T)))
			{
				return ComplexHelperMethods.ComplexEqualityOperator(left, right, ExpressionType.LessThan);
			}
			if (_lessthanFunction == null)
			{
				_lessthanFunction = GenericArithmeticFactory<T>.CreateGenericEqualityOperator(ExpressionType.LessThan);
			}
			return _lessthanFunction.Invoke(left, right);
		}

		public static bool GreaterThanOrEqual(T left, T right)
		{
			return (GreaterThan(left, right) || Equal(left, right));
		}

		public static bool LessThanOrEqual(T left, T right)
		{
			return (LessThan(left, right) || Equal(left, right));
		}

		public static bool Equal(T left, T right)
		{
			if (_equalFunction == null)
			{
				_equalFunction = GenericArithmeticFactory<T>.CreateGenericEqualityOperator(ExpressionType.Equal);
			}

			if (left == null)
			{
				return (right == null);
			}
			return _equalFunction.Invoke(left, right);
		}

		public static bool NotEqual(T left, T right)
		{
			return !Equal(left, right);
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
	}
}
