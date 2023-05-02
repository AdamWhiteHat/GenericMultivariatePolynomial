using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace ExtendedArithmetic
{

	/// <summary>
	/// Class Term.
	/// Implements the <see cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.Term{T}}" />
	/// Implements the <see cref="System.IEquatable{ExtendedArithmetic.Term{T}}" />
	/// Implements the <see cref="System.Collections.Generic.IEqualityComparer{ExtendedArithmetic.Term{T}}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.Term{T}}" />
	/// <seealso cref="System.IEquatable{ExtendedArithmetic.Term{T}}" />
	/// <seealso cref="System.Collections.Generic.IEqualityComparer{ExtendedArithmetic.Term{T}}" />
	public class Term<T> : ICloneable<Term<T>>, IEquatable<Term<T>>, IEqualityComparer<Term<T>>
	{
		/// <summary>
		/// Gets or sets the coefficient.
		/// </summary>
		public T CoEfficient { get; set; }

		/// <summary>
		/// An indexer into the indeterminates (variables).
		/// </summary>
		public Indeterminate[] Variables { get; private set; }

		/// <summary>
		/// Gets the degree of this term.
		/// </summary>
		public int Degree { get { return Variables.Any() ? Variables.Select(v => v.Exponent).Sum() : 0; } }

		/// <summary>
		/// Gets a the static value that represents the empty Term.
		/// </summary>
		public static Term<T> Empty = new Term<T>(GenericArithmetic<T>.Zero, Indeterminate.Empty);

		/// <summary>
		/// Gets a the static value that represents the zero Term.
		/// </summary>
		public static Term<T> Zero = new Term<T>(GenericArithmetic<T>.Zero, Indeterminate.Zero);

		#region Constructor & Parse

		/// <summary>
		/// Initializes a new instance of the <see cref="Term{T}"/> class, taking a coefficient and an array of indeterminates.
		/// </summary>
		/// <param name="coefficient">The coefficient.</param>
		/// <param name="variables">The variables.</param>
		public Term(T coefficient, Indeterminate[] variables)
		{
			CoEfficient = coefficient;
			Variables = CloneHelper<Indeterminate>.CloneCollection(variables).ToArray();
		}

		/// <summary>
		/// Constructs a new Term from its string representation.
		/// </summary>
		/// <exception cref="System.ArgumentException"></exception>
		internal static Term<T> Parse(string termString)
		{
			if (string.IsNullOrWhiteSpace(termString)) { throw new ArgumentException(); }

			string input = termString.Replace(" ", "");
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string[] parts = input.Split(new char[] { '*' });

			if (ComplexHelperMethods.IsComplexValueType(typeof(T)))
			{
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

			if (!variables.Any())
			{
				variables = Indeterminate.Empty;
			}

			return new Term<T>(coefficient, variables);
		}

		#endregion

		#region Internal Helper Methods

		/// <summary>
		/// Returns true if the two Terms share a common factor.
		/// </summary>		
		/// <exception cref="System.ArgumentNullException"></exception>
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

			if (GenericArithmetic<T>.Equal(right.CoEfficient, left.CoEfficient))
			{
				return true;
			}

			// 1 is a factor of every number.
			// Note that we have already determined it shares at least one variable by this point.
			if (GenericArithmetic<T>.Equal(right.CoEfficient, GenericArithmetic<T>.One))
			{
				return true;
			}

			if (!GenericArithmetic<T>.Equal(GenericArithmetic<T>.Modulo(left.CoEfficient, right.CoEfficient), GenericArithmetic<T>.Zero))
			{
				return false;
			}

			if (GenericArithmetic<T>.LessThanOrEqual(GenericArithmetic<T>.GCD(left.CoEfficient, right.CoEfficient), GenericArithmetic<T>.One))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the common divisors of two Terms.
		/// </summary>
		/// <param name="terms">The terms.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		internal static Tuple<T, char[]> GetCommonDivisors(params Term<T>[] terms)
		{
			if (terms.Any(t => t == null)) { throw new ArgumentNullException(); }

			T commonDivisors = GenericArithmetic<T>.GCD(terms.Select(trm => trm.CoEfficient));

			char[] commonVariables = terms.Select(trm => trm.Variables.SelectMany(indt => Enumerable.Repeat(indt.Symbol, indt.Exponent))
																				.OrderBy(c => c)
																				.ToList()
											)
											.Aggregate(Match<char>)
											.ToArray();

			return new Tuple<T, char[]>(commonDivisors, commonVariables);
		}

		/// <summary>
		/// Internal, generic match function.
		/// </summary>		
		internal static List<U> Match<U>(List<U> first, List<U> second)
		{
			List<U> smaller = first;
			List<U> larger = second;
			if (second.Count < first.Count)
			{
				smaller = second;
				larger = first;
			}

			List<U> results = new List<U>();
			foreach (U item in smaller)
			{
				U match = larger.FirstOrDefault(i => i.Equals(item));
				if (match != null)
				{
					larger.Remove(match);
					results.Add(match);
				}
			}

			return results;
		}

		/// <summary>
		/// Returns true if the two terms have identical indeterminates.
		/// </summary>		
		/// <exception cref="System.ArgumentNullException"></exception>
		internal static bool HasIdenticalIndeterminates(Term<T> left, Term<T> right)
		{
			if (left == null)
			{
				if (right == null) { return true; }
				throw new ArgumentNullException();
			}

			if (left.Degree == 0 && right.Degree == 0)
			{
				return true;
			}

			if (left.Variables.Length != right.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in left.Variables)
			{
				if (!variable.Equals(right.Variables[index])) { return false; }
				index++;
			}
			return true;
		}

		/// <summary>
		/// Returns true if this Term has and variables (indeterminants).
		/// </summary>		
		internal bool HasVariables()
		{
			if (!Variables.Any())
			{
				return false;
			}
			if (Variables.Length == 1 && Variables[0].Exponent == 0)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns the count of the terms's indeterminants.
		/// </summary>
		/// <returns>System.Int32.</returns>
		internal int VariableCount()
		{
			return Variables.Any() ? Variables.Where(v => v.Exponent != 0).Count() : 0;
		}

		#endregion

		#region Arithmetic

		/// <summary>
		/// Adds two terms together and produces a third.
		/// </summary>
		/// <exception cref="System.ArgumentException">Terms are incompatable for adding; Their indeterminates must match.</exception>
		public static Term<T> Add(Term<T> left, Term<T> right)
		{
			if (!HasIdenticalIndeterminates(left, right))
			{
				throw new ArgumentException("Terms are incompatable for adding; Their indeterminates must match.");
				//return Empty;
			}
			return new Term<T>(GenericArithmetic<T>.Add(left.CoEfficient, right.CoEfficient), left.Variables);
		}

		/// <summary>
		/// Subtracts two terms from each other, producing a third.
		/// </summary>
		public static Term<T> Subtract(Term<T> left, Term<T> right)
		{
			return Add(left, Negate(right));
		}

		/// <summary>
		/// Negates the supplied term.
		/// </summary>
		public static Term<T> Negate(Term<T> term)
		{
			return new Term<T>(GenericArithmetic<T>.Negate(term.CoEfficient), term.Variables);
		}

		/// <summary>
		/// Multiplies two terms together and returns the product.
		/// </summary>		
		public static Term<T> Multiply(Term<T> left, Term<T> right)
		{
			T resultCoefficient = GenericArithmetic<T>.Multiply(left.CoEfficient, right.CoEfficient);
			List<Indeterminate> resultVariables = new List<Indeterminate>();

			List<Indeterminate> rightVariables = right.Variables.ToList();

			foreach (var leftVar in left.Variables)
			{
				var matches = rightVariables.Where(indt => Indeterminate.AreCompatable(indt, leftVar)).ToList();
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

		/// <summary>
		/// Divides two terms, producing a quotient.
		/// </summary>
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

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public Term<T> Clone()
		{
			return new Term<T>(GenericArithmetic<T>.Clone(CoEfficient), CloneHelper<Indeterminate>.CloneCollection(Variables).ToArray());
		}

		/// <summary>
		/// Returns true if the two Terms are equal.
		/// </summary>
		public bool Equals(Term<T> other)
		{
			return this.Equals(this, other);
		}

		/// <summary>
		/// Returns true if the two Terms are equal.
		/// </summary>		
		public bool Equals(Term<T> x, Term<T> y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (!GenericArithmetic<T>.Equals(x.CoEfficient, y.CoEfficient)) { return false; }

			if (!x.Variables.Any() || (x.Variables.Length == 1 && x.Variables[0].Exponent == 0))
			{
				return (!y.Variables.Any() || (y.Variables.Length == 1 && y.Variables[0].Exponent == 0));
			}

			if (x.Variables.Length != y.Variables.Length) { return false; }

			int index = 0;
			foreach (Indeterminate variable in x.Variables)
			{
				if (!variable.Equals(y.Variables[index++])) { return false; }
			}
			return true;
		}

		/// <summary>
		/// Returns true if the two Terms are equal.
		/// </summary>		
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Term<T>);
		}

		/// <summary>
		/// Returns a hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		public int GetHashCode(Term<T> obj)
		{
			return obj.GetHashCode();
		}

		/// <summary>
		/// Returns a hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
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

		/// <summary>
		/// Combines two hash codes to make a third hash dependent on the the hash of the two properties combined.
		/// </summary>		
		internal static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		/// <summary>
		/// Converts the Term of the current instance to its equivalent string representation.
		/// </summary>
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

			bool variableStringEmpty = string.IsNullOrWhiteSpace(variableString);

			if (variableStringEmpty && GenericArithmetic<T>.Equal(GenericArithmetic<T>.Abs(CoEfficient), GenericArithmetic<T>.One))
			{
				coefficientString = CoEfficient.ToString();
			}

			if (!GenericArithmetic<T>.Equal(GenericArithmetic<T>.Abs(CoEfficient), GenericArithmetic<T>.One))
			{
				if (!variableStringEmpty)
				{
					multiplyString = "*";
				}
				coefficientString = CoEfficient.ToString();
			}
			else if (!variableStringEmpty && GenericArithmetic<T>.Sign(CoEfficient) == -1)
			{
				coefficientString = "-";
			}

			coefficientString = coefficientString.Replace("<-0;", "<0;");
			coefficientString = coefficientString.Replace("; -0>", "; 0>");

			coefficientString = coefficientString.Replace("<", "(");
			coefficientString = coefficientString.Replace(">", ")");
			coefficientString = coefficientString.Replace(";", ",");

			coefficientString = coefficientString.Replace("(-0,", "(0,");
			coefficientString = coefficientString.Replace(", -0)", ", 0)");

			return $"{coefficientString}{multiplyString}{variableString}";
		}

		#endregion

	}
}
