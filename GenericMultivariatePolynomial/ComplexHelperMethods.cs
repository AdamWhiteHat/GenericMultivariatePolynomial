using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using ExtendedArithmetic.Internal;
using System.Reflection;
using System.Linq.Expressions;

namespace ExtendedArithmetic
{
	public static class ComplexHelperMethods
	{
		public static bool IsComplexValueType(Type type)
		{
			return type.Name.Contains("Complex");
		}

		public static int ComplexGetRealPartSign<T>(T value)
		{
			PropertyInfo realProperty = typeof(T).GetProperty("Real", BindingFlags.Public | BindingFlags.Instance);
			Type realType = realProperty.PropertyType;

			object realValue = realProperty.GetValue(value);

			int realSign;

			MethodInfo signMethod = null;

			// System.Numerics.Complex must use: Math.Sign(instance.Real)
			if (GenericArithmeticCommon.IsArithmeticValueType(realType))
			{
				MethodInfo[] methods = typeof(Math).GetMethods(BindingFlags.Static | BindingFlags.Public);
				var absMethods = methods.Where(mi => mi.Name == "Sign");
				absMethods = absMethods.Where(mi => mi.GetParameters()[0].ParameterType == realType);
				signMethod = absMethods.FirstOrDefault();

				realSign = (int)signMethod.Invoke(null, new object[] { realValue });
			}

			else // ExtendedNumerics.BigComplex must use: instance.Real.Sign
			{
				PropertyInfo signProperty = realType.GetProperty("Sign", BindingFlags.Public | BindingFlags.Instance);

				realSign = (int)signProperty.GetValue(realValue);
			}

			return realSign;
		}

		public static object ComplexNegate(object value)
		{
			Type type = value.GetType();

			object result = null;
			MethodInfo negateMethod = null;
			if (GenericArithmeticCommon.IsArithmeticValueType(type))
			{
				ConstantExpression valueConstant = Expression.Constant(value, type);
				var negate = Expression.Negate(valueConstant);
				var lambdaExpression = Expression.Lambda(negate, Expression.Parameter(type));
				Delegate methodDelegate = lambdaExpression.Compile();
				result = methodDelegate.DynamicInvoke(value);
			}
			else
			{
				negateMethod = type.GetMethod("Negate", BindingFlags.Public | BindingFlags.Static);
				result = negateMethod.Invoke(null, new object[] { value });
			}

			return result;
		}

		public static bool ComplexEqualityOperator<T>(T left, T right, ExpressionType operationType)
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

			int leftRealSign = ComplexHelperMethods.ComplexGetRealPartSign(left);
			int rightRealSign = ComplexHelperMethods.ComplexGetRealPartSign(right);

			Delegate absFunction = GenericArithmeticFactory<T>.CreateAbsFunction_UnlikeReturnType();

			object abs_left = absFunction.DynamicInvoke(left);
			object abs_right = absFunction.DynamicInvoke(right);

			Type absResultType = absFunction.Method.ReturnType;

			if (leftRealSign == -1)
			{
				abs_left = ComplexHelperMethods.ComplexNegate(abs_left);
			}
			if (rightRealSign == -1)
			{
				abs_right = ComplexHelperMethods.ComplexNegate(abs_right);
			}

			MethodInfo compareMethod = absResultType.GetMethod("Compare", BindingFlags.Static | BindingFlags.Public);

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

		public static Complex Parse(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				throw new ArgumentException($"Argument {nameof(s)} cannot be null, empty or whitespace");
			}

			string input = new string(s.Where(c => !char.IsWhiteSpace(c)).ToArray());
			string[] parts = input.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length <= 0 || parts.Length > 2)
			{
				throw new FormatException($"Argument {nameof(s)} not of the correct format. Expecting format: \"(1.75, 3.5)\"");
			}

			double imaginary = 0;
			double real = double.Parse(parts[0]);

			if (parts.Length == 2)
			{
				imaginary = double.Parse(parts[1]);
			}

			return new Complex(real, imaginary);
		}
	}
}
