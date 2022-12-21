using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	public class Term<T> : ICloneable<Term<T>>, IEquatable<Term<T>>, IEqualityComparer<Term<T>>
	{
		public T CoEfficient { get; }
		public Indeterminate[] Variables { get; private set; }
		public int Degree { get { return Variables.Any() ? Variables.Select(v => v.Exponent).Sum() : 0; } }

		public static Term<T> Empty = new Term<T>(GenericArithmetic<T>.Zero, new Indeterminate[0]);
		internal static Term<T> Zero = new Term<T>(GenericArithmetic<T>.Zero, new Indeterminate[] { new Indeterminate('X', 0) });

		#region Constructor & Parse

		public Term(T coefficient, Indeterminate[] variables)
		{
			CoEfficient = coefficient;
			Variables = CloneHelper<Indeterminate>.CloneCollection(variables).ToArray();
		}

		internal static Term<T> Parse(string termString)
		{
			if (string.IsNullOrWhiteSpace(termString)) { throw new ArgumentException(); }

			string input = termString.Replace(" ", "");
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string[] parts = input.Split(new char[] { '*' });

			if (ComplexHelperMethods.IsComplexValueType(typeof(T)))
			{
				parts[0] = parts[0].Replace("<", "(");
				parts[0] = parts[0].Replace(">", ")");

				if (parts[0].Contains('(') && parts[0].Contains(')'))
				{
					if (parts[0].StartsWith("-"))
					{
						parts[0] = parts[0].Replace("-(", "(-");
					}
				}
			}

			T coefficient = GenericArithmetic<T>.One;
			bool parsed = false;
			try
			{
				coefficient = GenericArithmetic<T>.Parse(parts[0]);
				parts = parts.Skip(1).ToArray();
				parsed = true;
			}
			catch
			{
			}

			if (!parsed && parts[0].StartsWith("-"))
			{
				coefficient = GenericArithmetic<T>.MinusOne;
				parts[0] = parts[0].Replace("-", "");
			}

			Indeterminate[] variables = parts.Select(str => Indeterminate.Parse(str)).ToArray();

			return new Term<T>(coefficient, variables);
		}

		#endregion

		#region Internal Helper Methods

		internal static bool ShareCommonFactor(Term<T> left, Term<T> right)
		{
			if (left == null || right == null)
			{
				throw new ArgumentNullException();
			}
			if (!left.Variables.Any(lv => right.Variables.Any(rv => rv.Equals(lv))))
			{
				return false;
			}
			if (!GenericArithmetic<T>.Equal(right.CoEfficient, GenericArithmetic<T>.One) && (!GenericArithmetic<T>.Equal(GenericArithmetic<T>.Modulo(left.CoEfficient, right.CoEfficient), GenericArithmetic<T>.Zero)))
			{
				return false;
			}
			return true;
		}

		internal static bool AreIdentical(Term<T> left, Term<T> right)
		{
			if (left == null || right == null) { throw new ArgumentNullException(); }
			if (left.Variables.Length != right.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in left.Variables)
			{
				if (!variable.Equals(right.Variables[index])) { return false; }
				index++;
			}
			return true;
		}

		internal bool HasVariables()
		{
			return Variables.Any();
		}

		internal int VariableCount()
		{
			if (!HasVariables())
			{
				return 0;
			}
			else
			{
				return Variables.Length;
			}
		}

		#endregion

		#region Arithmetic

		public static Term<T> Add(Term<T> left, Term<T> right)
		{
			if (!AreIdentical(left, right))
			{
				//throw new ArgumentException("Terms are incompatable for adding; Their indeterminates must match.");
				return Empty;
			}
			return new Term<T>(GenericArithmetic<T>.Add(left.CoEfficient, right.CoEfficient), left.Variables);
		}

		public static Term<T> Subtract(Term<T> left, Term<T> right)
		{
			return Add(left, Negate(right));
		}

		public static Term<T> Negate(Term<T> term)
		{
			return new Term<T>(GenericArithmetic<T>.Negate(term.CoEfficient), term.Variables);
		}

		public static Term<T> Multiply(Term<T> left, Term<T> right)
		{
			T resultCoefficient = GenericArithmetic<T>.Multiply(left.CoEfficient, right.CoEfficient);
			List<Indeterminate> resultVariables = new List<Indeterminate>();

			List<Indeterminate> rightVariables = right.Variables.ToList();

			foreach (var leftVar in left.Variables)
			{
				var matches = rightVariables.Where(indt => indt.Symbol == leftVar.Symbol).ToList();
				if (matches.Any())
				{
					foreach (var rightMatch in matches)
					{
						rightVariables.Remove(rightMatch);
						resultVariables.Add(
							new Indeterminate(leftVar.Symbol, (leftVar.Exponent + rightMatch.Exponent))
						);
					}
				}
				else
				{
					resultVariables.Add(leftVar.Clone());
				}
			}

			if (rightVariables.Any())
			{
				foreach (var rightVar in rightVariables)
				{
					resultVariables.Add(rightVar.Clone());
				}
			}

			resultVariables = resultVariables.OrderBy(indt => indt.Symbol).ThenBy(indt => indt.Exponent).ToList();

			return new Term<T>(resultCoefficient, resultVariables.ToArray());
		}

		public static Term<T> Divide(Term<T> left, Term<T> right)
		{
			if (!Term<T>.ShareCommonFactor(left, right)) { return Empty; }

			T newCoefficient = GenericArithmetic<T>.Divide(left.CoEfficient, right.CoEfficient);

			List<Indeterminate> newVariables = new List<Indeterminate>();
			int max = left.Variables.Length;
			int index = 0;
			while (index < max)
			{
				if (index > right.Variables.Length - 1)
				{
					newVariables.Add(new Indeterminate(left.Variables[index].Symbol, left.Variables[index].Exponent));
				}
				else
				{
					if (left.Variables[index].Symbol == right.Variables[index].Symbol)
					{
						int newExponent = left.Variables[index].Exponent - right.Variables[index].Exponent;
						if (newExponent > 0)
						{
							newVariables.Add(new Indeterminate(left.Variables[index].Symbol, newExponent));
						}
					}
				}
				index++;
			}
			Term<T> result = new Term<T>(newCoefficient, newVariables.ToArray());
			return result;
		}

		#endregion

		#region Overrides and Interface implementations

		public Term<T> Clone()
		{
			return new Term<T>(GenericArithmetic<T>.Clone(CoEfficient), CloneHelper<Indeterminate>.CloneCollection(Variables).ToArray());
		}
		public bool Equals(Term<T> other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(Term<T> x, Term<T> y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (!GenericArithmetic<T>.Equals(x.CoEfficient, y.CoEfficient)) { return false; }
			if (!x.Variables.Any()) { return (!y.Variables.Any()) ? true : false; }
			if (x.Variables.Length != y.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in x.Variables)
			{
				if (!variable.Equals(y.Variables[index++])) { return false; }
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as Term<T>);
		}

		public int GetHashCode(Term<T> obj)
		{
			return obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			int hashCode = CoEfficient.GetHashCode();
			if (Variables.Any())
			{
				foreach (var variable in Variables)
				{
					hashCode = CombineHashCodes(hashCode, variable.GetHashCode());
				}
			}
			return hashCode;
		}

		internal static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		public override string ToString()
		{
			if (GenericArithmetic<T>.Equal(CoEfficient, GenericArithmetic<T>.Zero))
			{
				return "0";
			}

			string signString = string.Empty;
			string coefficientString = string.Empty;
			string variableString = string.Empty;
			string multiplyString = string.Empty;

			if (Variables.Any())
			{
				variableString = string.Join("*", Variables.Select(v => v.ToString()));
			}
			else if (GenericArithmetic<T>.Equal(GenericArithmetic<T>.Abs(CoEfficient), GenericArithmetic<T>.One))
			{
				coefficientString = CoEfficient.ToString();
			}

			if (!GenericArithmetic<T>.Equal(GenericArithmetic<T>.Abs(CoEfficient), GenericArithmetic<T>.One))
			{
				if (Variables.Any())
				{
					multiplyString = "*";
				}
				coefficientString = CoEfficient.ToString();
			}
			else if (Variables.Any() && GenericArithmetic<T>.Sign(CoEfficient) == -1)
			{
				coefficientString = "-";
			}

			return $"{coefficientString}{multiplyString}{variableString}";
		}

		#endregion

	}
}
