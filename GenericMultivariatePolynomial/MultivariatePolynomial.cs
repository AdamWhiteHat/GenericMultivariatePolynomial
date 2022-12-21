using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestGenericMultivariatePolynomial")]

namespace ExtendedArithmetic
{
	public class MultivariatePolynomial<T> : ICloneable<MultivariatePolynomial<T>>
	{
		public Term<T>[] Terms { get; private set; }
		public int Degree { get { return Terms.Any() ? Terms.Select(t => t.Degree).Max() : 0; } }

		#region Constructor & Parse

		public MultivariatePolynomial(Term<T>[] terms)
		{
			IEnumerable<Term<T>> newTerms = terms?.Where(trm => !GenericArithmetic<T>.Equal(trm.CoEfficient, GenericArithmetic<T>.Zero)) ?? new Term<T>[0];
			if (!newTerms.Any())
			{
				Terms = new Term<T>[] { Term<T>.Zero };
				return;
			}

			Terms = CloneHelper<Term<T>>.CloneCollection(newTerms).ToArray();
			OrderMonomials();
		}

		public static MultivariatePolynomial<T> Parse(string polynomialString)
		{
			string input = polynomialString;
			if (string.IsNullOrWhiteSpace(input)) { throw new ArgumentException(); }

			string inputString = input.Replace(" ", "");

			if (ComplexHelperMethods.IsComplexValueType(typeof(T)))
			{
				inputString = RewriteComplexPolynomial(inputString);
			}
			else
			{
				inputString = inputString.Replace("-", "+-");
				if (inputString.StartsWith("+-")) { inputString = new string(inputString.Skip(1).ToArray()); }
			}

			string[] stringTerms = inputString.Split(new char[] { '+' });

			if (!stringTerms.Any()) { throw new FormatException(); }

			Term<T>[] terms = stringTerms.Select(str => Term<T>.Parse(str)).ToArray();

			return new MultivariatePolynomial<T>(terms);
		}

		internal static string RewriteComplexPolynomial(string complexPolynomialString)
		{
			// Normally minus signs between terms is handled by replacing "-" with "+-" and splitting by "+" into terms.
			// However, complex numbers have the form (1, 0) and they break this assumption because
			// subtracting a complex term can be expressed like "X - (6, 0)*Y" or "X + (-6, 0)*Y" or even "X - 6*Y",
			// so we normalize complex polynomials here going character by character and using boolean flags to track state.

			// I wouldn't waste too much brain power trying to comprehend this code.
			// Just know that it replaces minus signs between terms with plus signs and makes the complex coefficient negative instead
			// and makes explicit coefficients with a value of 1.
			// E.g., converts this:
			// -3*X - Y - 1
			// Into this:
			// (-3, 0)*X + (-1, 0)*Y + (-1, 0)

			string inputString = complexPolynomialString.Replace(" ", "");

			bool inNumber = false;
			bool hasParentheses = false;
			bool inTerm = false;
			bool isNegated = false;
			int index = -1;
			List<char> outString = new List<char>();
			while (++index < inputString.Length)
			{
				char c = inputString[index];

				if (inNumber)
				{
					if (hasParentheses)
					{
						if (c == ')')
						{
							inNumber = false;
							hasParentheses = false;
						}
						else if (char.IsDigit(c) || "-,.".Contains(c))
						{
							// Do nothing
						}
					}

					else
					{
						if (c == '*')
						{
							inNumber = false;
						}
						else if (char.IsLetter(c))
						{
							inNumber = false;
							outString.Add('(');
							if (isNegated)
							{
								isNegated = false;
								outString.Add('-');
							}
							outString.Add('1');
							outString.Add(',');
							outString.Add('0');
							outString.Add(')');
							outString.Add('*');
						}
						else if ("+-".Contains(c))
						{
							inNumber = false;
							inTerm = false;
							if (c == '-')
							{
								isNegated = true;
								c = '+';
							}
						}
					}
				}
				else if (inTerm)
				{
					if (char.IsLetter(c) || "*^".Contains(c) || char.IsDigit(c))
					{
						// Do nothing
					}
					else if (c == '+')
					{
						inTerm = false;
					}
					else if (c == '-')
					{
						inTerm = false;
						isNegated = true;
						c = '+';
					}
				}
				else
				{
					if (c == '(' || char.IsDigit(c))
					{
						inNumber = true;
						inTerm = true;
						if (c == '(')
						{
							hasParentheses = true;
							if (isNegated)
							{
								outString.Add('(');
								c = '-';
								isNegated = false;
							}
						}
						else
						{
							if (isNegated)
							{
								outString.Add('-');
								isNegated = false;
							}
						}
					}
					else if (char.IsLetter(c))
					{
						inTerm = true;
						outString.Add('(');
						if (isNegated)
						{
							isNegated = false;
							outString.Add('-');
						}
						outString.Add('1');
						outString.Add(',');
						outString.Add('0');
						outString.Add(')');
						outString.Add('*');
					}
					else if (c == '-')
					{
						if (index != 0) { throw new FormatException(); }
						isNegated = true;
						continue;
					}
				}

				outString.Add(c);
			}

			return new string(outString.ToArray());
		}

		public static MultivariatePolynomial<T> GetDerivative(MultivariatePolynomial<T> poly, char symbol)
		{
			List<Term<T>> resultTerms = new List<Term<T>>();
			foreach (Term<T> term in poly.Terms)
			{
				if (term.Variables.Any() && term.Variables.Any(indt => indt.Symbol == symbol))
				{
					T newTerm_Coefficient = GenericArithmetic<T>.Zero;
					List<Indeterminate> newTerm_Variables = new List<Indeterminate>();

					foreach (Indeterminate variable in term.Variables)
					{
						if (variable.Symbol == symbol)
						{
							newTerm_Coefficient = GenericArithmetic<T>.Multiply(term.CoEfficient, GenericArithmetic<T>.Convert<int>(variable.Exponent));

							int newExponent = variable.Exponent - 1;
							if (newExponent > 0)
							{
								newTerm_Variables.Add(new Indeterminate(symbol, newExponent));
							}
						}
						else
						{
							newTerm_Variables.Add(variable.Clone());
						}
					}

					resultTerms.Add(new Term<T>(newTerm_Coefficient, newTerm_Variables.ToArray()));
				}
			}

			return new MultivariatePolynomial<T>(resultTerms.ToArray());
		}

		private void OrderMonomials()
		{
			if (Terms.Length > 1)
			{
				var a_noOrdering = Terms.ToList();
				var orderedEnumerable = Terms.OrderBy(t => t.Degree); // First by degree
				var b_byDegree = orderedEnumerable.ToList();
				List<int> degrees = orderedEnumerable.Select(t => t.Degree).ToList();

				orderedEnumerable = orderedEnumerable.ThenByDescending(t => t.VariableCount()); // Then by variable count
				var c_byVariableCount = orderedEnumerable.ToList();
				List<int> variableCounts = orderedEnumerable.Select(t => t.VariableCount()).ToList();

				List<Term<T>> d_byCoefficient;
				Type tType = typeof(T);
				Type iComparableType = tType.GetInterface("IComparable");
				if (iComparableType != null)
				{
					orderedEnumerable = orderedEnumerable.ThenBy(t => t.CoEfficient); // Then by coefficient value					
					d_byCoefficient = orderedEnumerable.ToList();
				}
				else
				{
					if (ComplexHelperMethods.IsComplexValueType(tType))
					{
						orderedEnumerable = orderedEnumerable.ThenBy(t => t.CoEfficient, new ComplexComparer<T>());
						d_byCoefficient = orderedEnumerable.ToList();
					}
				}

				orderedEnumerable = orderedEnumerable
					.ThenByDescending(t =>
						new string(t.Variables.OrderBy(v => v.Symbol).Select(v => v.Symbol).ToArray())
					); // Lastly, lexicographic order of variables. Descending order because zero degree terms (smaller stuff) goes first.

				var e_bySymbols = orderedEnumerable.ToList();

				Terms = orderedEnumerable.ToArray();
			}
		}

		internal bool HasVariables()
		{
			return this.Terms.Any(t => t.HasVariables());
		}

		internal T MaxCoefficient()
		{
			if (HasVariables())
			{
				var termsWithVariables = this.Terms.Select(t => t).Where(t => t.HasVariables());
				return termsWithVariables.Select(t => t.CoEfficient).Max();
			}
			return GenericArithmetic<T>.MinusOne;
		}

		#endregion

		#region Evaluate

		public T Evaluate(List<Tuple<char, T>> indeterminateValues)
		{
			T result = GenericArithmetic<T>.Zero;
			foreach (Term<T> term in Terms)
			{
				T termValue = term.CoEfficient;

				if (term.Variables.Any())
				{
					var variableValues =
						term.Variables
						.Select(indetrmnt =>
							indeterminateValues.Where(tup => tup.Item1 == indetrmnt.Symbol)
											  .Select(tup => GenericArithmetic<T>.Power(tup.Item2, indetrmnt.Exponent))
											  .Single()
						);

					termValue = GenericArithmetic<T>.Multiply(termValue, variableValues.Aggregate(GenericArithmetic<T>.Multiply));
				}

				result = GenericArithmetic<T>.Add(result, termValue);
			}
			return result;
		}

		/// <summary>
		/// Like the Evaluate method, except it replaces indeterminates with Polynomials instead of integers,
		/// and returns the resulting (usually large) Polynomial
		/// </summary>
		public MultivariatePolynomial<T> FunctionalComposition(List<Tuple<char, MultivariatePolynomial<T>>> indeterminateValues)
		{
			List<Term<T>> terms = this.Terms.ToList();
			List<MultivariatePolynomial<T>> composedTerms = new List<MultivariatePolynomial<T>>();

			foreach (Term<T> trm in terms)
			{
				MultivariatePolynomial<T> constant = MultivariatePolynomial<T>.Parse(trm.CoEfficient.ToString());
				List<MultivariatePolynomial<T>> toCompose = new List<MultivariatePolynomial<T>>();
				toCompose.Add(constant.Clone());
				foreach (Indeterminate variable in trm.Variables)
				{
					int exp = variable.Exponent;
					MultivariatePolynomial<T> valueOfIndeterminate = indeterminateValues.Where(tup => tup.Item1 == variable.Symbol).Select(tup => tup.Item2).FirstOrDefault();
					if (valueOfIndeterminate == null)
					{
						MultivariatePolynomial<T> thisVariableAsPoly = new MultivariatePolynomial<T>(new Term<T>[] { new Term<T>(GenericArithmetic<T>.One, new Indeterminate[] { variable }) });
						toCompose.Add(thisVariableAsPoly);
					}
					else if (exp == 0) { continue; }
					else if (exp == 1)
					{
						toCompose.Add(valueOfIndeterminate);
					}
					else
					{
						MultivariatePolynomial<T> toMultiply = MultivariatePolynomial<T>.Pow(valueOfIndeterminate, exp);
						toCompose.Add(toMultiply);
					}
				}
				MultivariatePolynomial<T> composed = MultivariatePolynomial<T>.Product(toCompose);
				composedTerms.Add(composed);
			}

			MultivariatePolynomial<T> result = MultivariatePolynomial<T>.Sum(composedTerms);
			return result;
		}

		#endregion

		#region Arithmetic

		public static MultivariatePolynomial<T> GCD(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			MultivariatePolynomial<T> dividend = left.Clone();
			MultivariatePolynomial<T> divisor = right.Clone();
			MultivariatePolynomial<T> quotient;
			MultivariatePolynomial<T> remainder;
			T dividendLeadingCoefficient = GenericArithmetic<T>.Zero;
			T divisorLeadingCoefficient = GenericArithmetic<T>.Zero;

			bool swap = false;

			do
			{
				swap = false;

				dividendLeadingCoefficient = dividend.Terms.Last().CoEfficient;
				divisorLeadingCoefficient = divisor.Terms.Last().CoEfficient;

				if (dividend.Degree < divisor.Degree)
				{
					swap = true;
				}
				else if ((dividend.Degree == divisor.Degree) && GenericArithmetic<T>.LessThan(dividendLeadingCoefficient, divisorLeadingCoefficient))
				{
					swap = true;
				}

				if (swap)
				{
					MultivariatePolynomial<T> temp = dividend.Clone();
					dividend = divisor;
					divisor = temp.Clone();
				}

				quotient = MultivariatePolynomial<T>.Divide(dividend, divisor);
				dividend = quotient.Clone();

			}
			while (GenericArithmetic<T>.GreaterThan(GenericArithmetic<T>.Abs(dividendLeadingCoefficient), GenericArithmetic<T>.Zero) && GenericArithmetic<T>.GreaterThan(GenericArithmetic<T>.Abs(divisorLeadingCoefficient), GenericArithmetic<T>.Zero) && dividend.HasVariables() && divisor.HasVariables());

			if (dividend.HasVariables())
			{
				return divisor.Clone();
			}
			else
			{
				return dividend.Clone();
			}
		}

		public static MultivariatePolynomial<T> Sum(IEnumerable<MultivariatePolynomial<T>> polys)
		{
			MultivariatePolynomial<T> result = null;
			foreach (MultivariatePolynomial<T> p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = MultivariatePolynomial<T>.Add(result, p);
				}
			}
			return result;
		}

		public static MultivariatePolynomial<T> Product(IEnumerable<MultivariatePolynomial<T>> polys)
		{
			MultivariatePolynomial<T> result = null;
			foreach (MultivariatePolynomial<T> p in polys)
			{
				if (result == null)
				{
					result = p.Clone();
				}
				else
				{
					result = MultivariatePolynomial<T>.Multiply(result, p);
				}
			}
			return result;
		}

		public static MultivariatePolynomial<T> Add(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			return OneToOneArithmetic(left, right, Term<T>.Add);
		}

		public static MultivariatePolynomial<T> Subtract(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			return OneToOneArithmetic(left, right, Term<T>.Subtract);
		}

		private static MultivariatePolynomial<T> OneToOneArithmetic(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right, Func<Term<T>, Term<T>, Term<T>> operation)
		{
			List<Term<T>> leftTermsList = CloneHelper<Term<T>>.CloneCollection(left.Terms).ToList();

			foreach (Term<T> rightTerm in right.Terms)
			{
				var match = leftTermsList.Where(leftTerm => Term<T>.AreIdentical(leftTerm, rightTerm));
				if (match.Any())
				{
					Term<T> matchTerm = match.Single();
					leftTermsList.Remove(matchTerm);

					Term<T> result = operation.Invoke(matchTerm, rightTerm);
					if (!GenericArithmetic<T>.Equal(result.CoEfficient, GenericArithmetic<T>.Zero))
					{
						if (!leftTermsList.Any(lt => lt.Equals(result)))
						{
							leftTermsList.Add(result);
						}
					}
				}
				else
				{
					if (operation == Term<T>.Subtract)
					{
						leftTermsList.Add(Term<T>.Negate(rightTerm));
					}
					else
					{
						leftTermsList.Add(rightTerm);
					}
				}
			}
			return new MultivariatePolynomial<T>(leftTermsList.ToArray());
		}

		public static MultivariatePolynomial<T> Multiply(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			List<Term<T>> resultTerms = new List<Term<T>>();

			foreach (var leftTerm in left.Terms)
			{
				foreach (var rightTerm in right.Terms)
				{
					Term<T> newTerm = Term<T>.Multiply(leftTerm, rightTerm);

					// Combine like terms
					var likeTerms = resultTerms.Where(trm => Term<T>.AreIdentical(newTerm, trm));
					if (likeTerms.Any())
					{
						resultTerms = resultTerms.Except(likeTerms).ToList();

						Term<T> likeTermsSum = likeTerms.Aggregate(Term<T>.Add);
						Term<T> sum = Term<T>.Add(newTerm, likeTermsSum);

						newTerm = sum;
					}

					// Add new term to resultTerms
					resultTerms.Add(newTerm);
				}
			}

			return new MultivariatePolynomial<T>(resultTerms.ToArray());
		}

		public static MultivariatePolynomial<T> Pow(MultivariatePolynomial<T> poly, int exponent)
		{
			if (exponent < 0)
			{
				throw new NotImplementedException("Raising a polynomial to a negative exponent not supported.");
			}
			else if (exponent == 0)
			{
				return new MultivariatePolynomial<T>(new Term<T>[] { new Term<T>(GenericArithmetic<T>.One, new Indeterminate[0]) });
			}
			else if (exponent == 1)
			{
				return poly.Clone();
			}

			MultivariatePolynomial<T> result = poly.Clone();

			int counter = exponent - 1;
			while (counter != 0)
			{
				result = MultivariatePolynomial<T>.Multiply(result, poly);
				counter -= 1;
			}
			return new MultivariatePolynomial<T>(result.Terms);
		}

		public static MultivariatePolynomial<T> Divide(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			List<Term<T>> newTermsList = new List<Term<T>>();
			List<Term<T>> leftTermsList = CloneHelper<Term<T>>.CloneCollection(left.Terms).ToList();

			foreach (Term<T> rightTerm in right.Terms)
			{
				var matches = leftTermsList.Where(leftTerm => Term<T>.ShareCommonFactor(leftTerm, rightTerm)).ToList();
				if (matches.Any())
				{
					foreach (Term<T> matchTerm in matches)
					{
						leftTermsList.Remove(matchTerm);
						Term<T> quotient = Term<T>.Divide(matchTerm, rightTerm);
						if (quotient != Term<T>.Empty)
						{
							if (!newTermsList.Any(lt => lt.Equals(quotient)))
							{
								newTermsList.Add(quotient);
							}
						}
					}
				}
				else
				{
					///newTermsList.Add(rightTerm);
				}
			}
			MultivariatePolynomial<T> result = new MultivariatePolynomial<T>(newTermsList.ToArray());
			return result;
		}

		#endregion

		#region Overrides and Interface implementations

		public MultivariatePolynomial<T> Clone()
		{
			return new MultivariatePolynomial<T>(CloneHelper<Term<T>>.CloneCollection(Terms).ToArray());
		}

		public bool Equals(MultivariatePolynomial<T> other)
		{
			return this.Equals(this, other);
		}

		public bool Equals(MultivariatePolynomial<T> x, MultivariatePolynomial<T> y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (!x.Terms.Any()) { return (!y.Terms.Any()) ? true : false; }
			if (x.Terms.Length != y.Terms.Length) { return false; }
			if (x.Degree != y.Degree) { return false; }

			int index = 0;
			foreach (Term<T> term in x.Terms)
			{
				if (!term.Equals(y.Terms[index++])) { return false; }
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as MultivariatePolynomial<T>);
		}

		public int GetHashCode(MultivariatePolynomial<T> obj)
		{
			return obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			int hashCode = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
			if (Terms.Any())
			{
				foreach (var term in Terms)
				{
					hashCode = Term<T>.CombineHashCodes(hashCode, term.GetHashCode());
				}
			}
			return hashCode;
		}

		public override string ToString()
		{
			string signString = string.Empty;
			string termString = string.Empty;
			string result = string.Empty;

			result = string.Join(" + ", Terms.Reverse().Select(trm => trm.ToString()));
			result = result.Replace(" + -", " - ");

			return result;
		}

		#endregion

	}
}
