using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestGenericMultivariatePolynomial, PublicKey=002400000480000094010000060200000024000052534131000c000001000100f1f32f46310ef0c3987a3e6812202bc63106aee11d54c0bc71fca3a5efe5024fea6ff66885f1d3044c990b0ac400043125cc9f995f4816c82f4a838161a795d00b0fd5cc4501871dcafc936daa752aee2607b2cc7cf879ec3d20001acf325ec95fa38a62b83e0c7cbe22272b9e81a490c28ef2c3fadf8b293fa6026d4bb04d9298093b197434fffe82a5797705165a3a68c6b6c8868717f5885fa6c58cbb45a5d4afc74252986e1bad6c5eec4310bee87d9395d11b6612c3b83268f62b585b5140ad443563984d05909f581c73b3bfaa8ce437407c9f3c973ebeb3fb7289064f57bbcd5e9008e505daf17e06bc298bad9a7ceccfc3bd032387c2f094baf20f66bb37db398c3d8ba77b0bfcd67da8aaf039ef61260ccb1b2e6b06534303c5ce7d34c5d73ba7d12ad1d1d975478dd2a72ba7e3dc58eea7e311921030c2e4793d0db2401fe3556f738fdb4b9193e6e25b0bddd99a29bee36d87cce287a3b757ccb7e0174c5c338f1a3c28ced3385e0a345b6e0a58a919559bf242b0abc3d1af94a2")]

namespace ExtendedArithmetic
{
	/// <summary>
	/// Class MultivariatePolynomial.
	/// Implements the <see cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.MultivariatePolynomial{T}}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.MultivariatePolynomial{T}}" />
	public class MultivariatePolynomial<T> : ICloneable<MultivariatePolynomial<T>>
	{
		/// <summary>
		/// Indexer for the polynomial Terms
		/// </summary>
		public Term<T>[] Terms { get; private set; }

		/// <summary>
		/// Gets the degree of the polynomial.
		/// </summary>
		public int Degree { get { return Terms.Any() ? Terms.Select(t => t.Degree).Max() : 0; } }

		#region Constructor & Parse

		/// <summary>
		/// Initializes a new instance of the <see cref="MultivariatePolynomial{T}"/> class from an array of Terms.
		/// </summary>
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

		/// <summary>
		/// Constructs a polynomial from its string representation.
		/// Parse does not allow implicit multiplication symbols.		
		/// Terms with a coefficient of zero may be omitted.
		/// Indetermenents raised to the zeroth power may be omitted.
		/// Indetermenents raised to the first power may omit the exponentiation symbol and the one.
		/// Whitespace is ignored.
		/// For example, these two polynomials are identical:
		/// 4*X^2*Y^3 + Y^2 + Z + 1
		/// 4*X^2*Y^3 + 1*Y^2 + 1*Z^1 + 1*X^0
		/// </summary>
		/// <param name="polynomialString">The string to parse.</param>
		/// <returns>MultivariatePolynomial&lt;T&gt;.</returns>
		/// <exception cref="System.ArgumentException"></exception>
		/// <exception cref="System.FormatException"></exception>
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

		/// <summary>
		/// Monomial ordering method.
		/// Must produce a unique and deterministic ordering or else the multivariate arithmetic will give inconsistant answers.
		/// </summary>
		private void OrderMonomials()
		{
			if (Terms.Length > 1)
			{
				var a_noOrdering = Terms.ToList();
				var orderedEnumerable = Terms.OrderByDescending(t => t.Degree); // First by degree
#if DEBUG
				var b_byDegree = orderedEnumerable.ToList();
				List<int> degrees = orderedEnumerable.Select(t => t.Degree).ToList();
#endif

				orderedEnumerable = orderedEnumerable.ThenBy(t => t.VariableCount()); // Then by variable count
#if DEBUG
				var c_byVariableCount = orderedEnumerable.ToList();
				List<int> variableCounts = orderedEnumerable.Select(t => t.VariableCount()).ToList();
#endif

				List<Term<T>> d_byCoefficient;
				Type tType = typeof(T);
				Type iComparableType = tType.GetInterface("IComparable");
				if (iComparableType != null)
				{
					orderedEnumerable = orderedEnumerable.ThenByDescending(t => t.CoEfficient); // Then by coefficient value					
#if DEBUG
					d_byCoefficient = orderedEnumerable.ToList();
#endif
				}
				else
				{
					if (ComplexHelperMethods.IsComplexValueType(tType))
					{
						orderedEnumerable = orderedEnumerable.ThenByDescending(t => t.CoEfficient, new ComplexComparer<T>());
#if DEBUG
						d_byCoefficient = orderedEnumerable.ToList();
#endif
					}
				}

				orderedEnumerable = orderedEnumerable
					.ThenBy(t =>
						new string(t.Variables.OrderBy(v => v.Symbol).Select(v => v.Symbol).ToArray())
					); // Lastly, lexicographic order of variables. Descending order because zero degree terms (smaller stuff) goes first.

#if DEBUG
				var e_bySymbols = orderedEnumerable.ToList();
#endif

				Terms = orderedEnumerable.ToArray();
			}
		}

		/// <summary>
		/// Determines whether this instance has variables (Indeterminates).
		/// </summary>
		internal bool HasVariables()
		{
			return this.Terms.Any(t => t.HasVariables());
		}

		/// <summary>
		/// Returns the largest coefficient of all the Terms.
		/// </summary>
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

		/// <summary>
		/// Evaluates the polynomial at the specified indeterminate values.
		/// </summary>		
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
											  .Select(tup => GenericArithmetic<T>.PowerInt(tup.Item2, indetrmnt.Exponent))
											  .FirstOrDefault()
						);

					if (variableValues.Any())
					{
						termValue = GenericArithmetic<T>.Multiply(termValue, variableValues.Aggregate(GenericArithmetic<T>.Multiply));
					}
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


		/// <summary>
		/// GCDs the specified left.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">left</exception>
		/// <exception cref="System.ArgumentNullException">right</exception>
		public static MultivariatePolynomial<T> GCD(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			// TODO: This method needs to employ several strategies in order to perform GCD:
			//
			// IF
			//		The polynomial only contains 1 indeterminant,
			//		that is, if the polynomial is actually univariate
			// THEN
			//		Attempt to factor the polynomial into roots (X + 1)(X + 3)(X + 6)
			//		by "factoring" (i.e. apply the Factor method of my univariate polynomial library ExtendedArithmetic.Polynomial)
			//		Remove from the dividend matching roots in the divisor.
			// ELSE
			//		Groebner basis methods?
			//

			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			List<char> leftSymbols = left.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> rightSymbols = right.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> combinedSymbols = leftSymbols.Concat(rightSymbols).Distinct().ToList();

			// If polynomial is actually univariate
			if (combinedSymbols.Count <= 1)
			{
				List<MultivariatePolynomial<T>> leftFactors = Factor(left);
				List<MultivariatePolynomial<T>> rightFactors = Factor(right);

				Console.WriteLine("Left.Factors():");
				Console.WriteLine(string.Join(Environment.NewLine, leftFactors));
				Console.WriteLine("");

				Console.WriteLine("Right.Factors():");
				Console.WriteLine(string.Join(Environment.NewLine, rightFactors));
				Console.WriteLine();


				List<MultivariatePolynomial<T>> smaller = leftFactors;
				List<MultivariatePolynomial<T>> larger = rightFactors;
				if (leftFactors.Count > rightFactors.Count)
				{
					smaller = rightFactors;
					larger = leftFactors;
				}

				List<MultivariatePolynomial<T>> common = new List<MultivariatePolynomial<T>>();
				foreach (var factor in smaller)
				{
					if (larger.Contains(factor))
					{
						common.Add(factor);
					}
				}

				MultivariatePolynomial<T> product = MultivariatePolynomial<T>.Parse("1");

				foreach (var poly in common)
				{
					product = MultivariatePolynomial<T>.Multiply(product, poly);
				}

				return product;
			}
			else
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
		}

		/// <summary>
		/// Factors the specified polynomial.
		/// </summary>
		public static List<MultivariatePolynomial<T>> Factor(MultivariatePolynomial<T> polynomial)
		{
			if (polynomial == null) throw new ArgumentNullException(nameof(polynomial));

			List<MultivariatePolynomial<T>> results = new List<MultivariatePolynomial<T>>();

			List<char> symbols = polynomial.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			if (symbols.Count >= 1) // Rational root theorem, essentially
			{
				char symbol = symbols.First();

				MultivariatePolynomial<T> remainingPoly = polynomial.Clone();

				IEnumerable<T> coefficients = remainingPoly.Terms.Select(trm => trm.CoEfficient);

				T gcd = GenericArithmetic<T>.GCD(coefficients);
				if (GenericArithmetic<T>.GreaterThan(gcd, GenericArithmetic<T>.One))
				{
					MultivariatePolynomial<T> gcdPoly = MultivariatePolynomial<T>.Parse(gcd.ToString());
					results.Add(gcdPoly);
					remainingPoly = Divide(remainingPoly, gcdPoly);
				}

				if (remainingPoly.Degree == 0)
				{
					return results;
				}

				int resultCount = -1;

				while (remainingPoly.Degree > 0)
				{
					if (resultCount == results.Count)
					{
						break;
					}

					var leadingCoeffQ = remainingPoly.Terms.First().CoEfficient;
					var constantCoeffP = remainingPoly.Terms.Last().CoEfficient;

					var constantDivisors = GenericArithmetic<T>.GetAllDivisors(constantCoeffP).ToList();
					var leadingDivisors = GenericArithmetic<T>.GetAllDivisors(leadingCoeffQ).ToList();

					// <(denominator/numerator), numerator, denominator>
					List<Tuple<T, T, T>> candidates =
						constantDivisors.SelectMany(n => leadingDivisors.SelectMany(d =>
						{
							List<Tuple<T, T, T>> selected = new List<Tuple<T, T, T>>();
							T num1 = n;
							T num2 = GenericArithmetic<T>.Negate(n);
							T denom = d;

							T quotient1 = GenericArithmetic<T>.Divide(num1, denom);
							T quotient2 = GenericArithmetic<T>.Divide(num2, denom);
							selected.Add(new Tuple<T, T, T>(quotient1, num1, denom));
							selected.Add(new Tuple<T, T, T>(quotient2, num2, denom));

							return selected;
						})).ToList();

					candidates = candidates.OrderBy(tup => GenericArithmetic<T>.Abs(tup.Item1))
										   .ThenByDescending(tup => GenericArithmetic<T>.Sign(tup.Item1))
										   .ToList();

					resultCount = results.Count;

					foreach (Tuple<T, T, T> candidate in candidates)
					{
						T evalResult = remainingPoly.Evaluate(new List<Tuple<char, T>>() { new Tuple<char, T>(symbol, candidate.Item1) });
						bool isRoot = GenericArithmetic<T>.Equal(evalResult, GenericArithmetic<T>.Zero);

						if (!isRoot)
						{
							continue;
						}

						string rootMonomial = $"{candidate.Item3}*{symbol} {(GenericArithmetic<T>.Sign(GenericArithmetic<T>.Negate(candidate.Item2)) == -1 ? "-" : "+")} {GenericArithmetic<T>.Abs(candidate.Item2)}";
						MultivariatePolynomial<T> factor = MultivariatePolynomial<T>.Parse(rootMonomial);

						remainingPoly = Divide(remainingPoly, factor);
						results.Add(factor);

						break;
					}
				}
			}
			else
			{

			}
			return results;
		}

		/// <summary>
		/// Sums a collection of polynomials.
		/// </summary>
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

		/// <summary>
		/// Products a collection of polynomials.
		/// </summary>
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

		/// <summary>
		/// Multivariate Polynomial Addition
		/// </summary>		
		public static MultivariatePolynomial<T> Add(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			return OneToOneArithmetic(left, right, Term<T>.Add);
		}

		/// <summary>
		/// Multivariate Polynomial Subtraction.
		/// </summary>
		public static MultivariatePolynomial<T> Subtract(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			return OneToOneArithmetic(left, right, Term<T>.Subtract);
		}

		/// <summary>
		/// Takes the Terms pair-wise from the two polynomials and performs some operation on them.
		/// </summary>		
		private static MultivariatePolynomial<T> OneToOneArithmetic(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right, Func<Term<T>, Term<T>, Term<T>> operation)
		{
			List<Term<T>> leftTermsList = CloneHelper<Term<T>>.CloneCollection(left.Terms).ToList();

			foreach (Term<T> rightTerm in right.Terms)
			{
				var match = leftTermsList.Where(leftTerm => Term<T>.HasIdenticalIndeterminates(leftTerm, rightTerm));
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

		/// <summary>
		/// Multivariate Polynomial Multiplication
		/// </summary>
		public static MultivariatePolynomial<T> Multiply(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			List<Term<T>> resultTerms = new List<Term<T>>();

			foreach (var leftTerm in left.Terms)
			{
				foreach (var rightTerm in right.Terms)
				{
					Term<T> newTerm = Term<T>.Multiply(leftTerm, rightTerm);

					// Combine like terms
					var likeTerms = resultTerms.Where(trm => Term<T>.HasIdenticalIndeterminates(newTerm, trm));
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

		/// <summary>
		/// Multivariate Polynomial Exponentiation
		/// </summary>
		/// <exception cref="System.NotImplementedException">Raising a polynomial to a negative exponent not supported.</exception>
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

		/// <summary>
		/// Multivariate Polynomial Division.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">left</exception>
		/// <exception cref="System.ArgumentNullException">right</exception>
		public static MultivariatePolynomial<T> Divide(MultivariatePolynomial<T> left, MultivariatePolynomial<T> right)
		{
			// Because multivariate polynomial division is a whole different beast,
			// this method handles two different cases:
			// 1) Where both polynomials contain 1 or less variables and where both of those variables are the same (when applicable)
			// 2) Where one or both polynomials are multivariate (i.e. All other cases)
			//
			//

			if (left == null) throw new ArgumentNullException(nameof(left));
			if (right == null) throw new ArgumentNullException(nameof(right));

			List<char> leftSymbols = left.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> rightSymbols = right.Terms.SelectMany(t => t.Variables.Where(v => v.Exponent > 0).Select(v => v.Symbol)).Distinct().ToList();
			List<char> combinedSymbols = leftSymbols.Concat(rightSymbols).Distinct().ToList();

			// If polynomial is actually univariate
			if (combinedSymbols.Count <= 1)
			{
				if (right.Degree > left.Degree)
				{
					return left;
				}

				int rightDegree = right.Degree;
				int quotientDegree = (left.Degree - rightDegree) + 1;

				// Turn an array of Terms into an array of coefficients, including terms with coefficient of zero,
				// such that index into the array encodes its degree/exponent
				T[] rem = Enumerable.Repeat(GenericArithmetic<T>.Zero, left.Degree + 1).ToArray();
				foreach (Term<T> t in left.Terms)
				{
					rem[t.Degree] = t.CoEfficient;
				}
				// Turn an array of Terms into an array of coefficients
				T[] rightCoeffs = Enumerable.Repeat(GenericArithmetic<T>.Zero, rightDegree + 1).ToArray();
				foreach (Term<T> t in right.Terms)
				{
					rightCoeffs[t.Degree] = t.CoEfficient;
				}
				// Array of coefficients to hold our result.
				T[] quotient = Enumerable.Repeat(GenericArithmetic<T>.Zero, quotientDegree + 1).ToArray();

				T leadingCoefficent = rightCoeffs[rightDegree];

				// The leading coefficient is the only number we ever divide by
				// (so if right is monic, polynomial division does not involve division at all!)
				for (int i = quotientDegree - 1; i >= 0; i--)
				{
					quotient[i] = GenericArithmetic<T>.Divide(rem[rightDegree + i], leadingCoefficent);
					rem[rightDegree + i] = GenericArithmetic<T>.Zero;

					for (int j = rightDegree + i - 1; j >= i; j--)
					{
						rem[j] = GenericArithmetic<T>.Subtract(rem[j], GenericArithmetic<T>.Multiply(quotient[i], rightCoeffs[j - i]));
					}
				}

				// Turn array of coefficients into array of terms.
				char symbol = 'X';
				if (combinedSymbols.Any())
				{
					symbol = combinedSymbols.First();
				}
				List<Term<T>> newTerms = new List<Term<T>>();
				int index = -1;
				foreach (T q in quotient)
				{
					index++;

					if (!GenericArithmetic<T>.Equal(q, GenericArithmetic<T>.Zero))
					{
						Term<T> newTerm;

						if (index == 0)
						{
							newTerm = new Term<T>(q, Indeterminate.Empty);
						}
						else
						{
							newTerm = new Term<T>(q, new Indeterminate[] { new Indeterminate(symbol, index) });
						}

						newTerms.Add(newTerm);
					}
				}

				return new MultivariatePolynomial<T>(newTerms.ToArray());
			}
			else // All other cases (i.e. actually multivariate)
			{
				List<Term<T>> newTermsList = new List<Term<T>>();
				List<Term<T>> leftTermsList = CloneHelper<Term<T>>.CloneCollection(left.Terms).ToList();
				List<Term<T>> rightTermsList = CloneHelper<Term<T>>.CloneCollection(right.Terms).ToList();

				foreach (Term<T> rightTerm in rightTermsList)
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
		}

		#endregion

		#region Change Forms

		/// <summary>
		/// Gets the derivative of a polynomial with respect to a indeterminant.
		/// </summary>
		/// <param name="poly">The poly to find the derivative of.</param>
		/// <param name="symbol">The symbol with witch to take the derivative with respect to.</param>
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


		/// <summary>
		/// Finds the indefinite integral of the polynomial.
		/// </summary>
		/// <param name="c">The constant.</param>
		/// <returns>The indefinite integral.</returns>
		public static MultivariatePolynomial<T> IndefiniteIntegral(MultivariatePolynomial<T> poly, char symbol, T c)
		{
			List<Term<T>> newTerms = new List<Term<T>>();

			foreach (Term<T> term in poly.Terms.ToList())
			{
				int divisor = 1;
				bool symbolAdded = false;
				List<Indeterminate> newVariables = new List<Indeterminate>();
				foreach (Indeterminate indeterminate in term.Variables)
				{
					char newSymbol = indeterminate.Symbol;
					int newExponent = indeterminate.Exponent;
					if (indeterminate.Symbol == symbol)
					{
						divisor = newExponent + 1;
						newExponent = divisor;
						symbolAdded = true;
					}
					newVariables.Add(new Indeterminate(newSymbol, newExponent));
				}
				if (!symbolAdded)
				{
					newVariables.Add(new Indeterminate(symbol, 1));
				}

				T newCoefficient = term.CoEfficient;
				if (divisor > 1)
				{
					newCoefficient = GenericArithmetic<T>.Divide(newCoefficient, GenericArithmetic<T>.Convert(divisor));
				}

				newTerms.Add(new Term<T>(newCoefficient, newVariables.ToArray()));
			}

			return new MultivariatePolynomial<T>(newTerms.ToArray());
		}

		#endregion

		#region Overrides and Interface implementations

		/// <summary>
		/// Clones this instance.
		/// Rebuilds the polynomial with all new variables.
		/// </summary>
		public MultivariatePolynomial<T> Clone()
		{
			return new MultivariatePolynomial<T>(CloneHelper<Term<T>>.CloneCollection(Terms).ToArray());
		}

		/// <summary>
		/// Returns true if the two polynomials are equal.
		/// </summary>	
		public bool Equals(MultivariatePolynomial<T> other)
		{
			return this.Equals(this, other);
		}

		/// <summary>
		/// Returns true if the two polynomials are equal.
		/// </summary>
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

		/// <summary>
		/// Returns true if the two polynomials are equal.
		/// </summary>
		public override bool Equals(object obj)
		{
			return this.Equals(obj as MultivariatePolynomial<T>);
		}

		/// <summary>
		/// Returns a hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		public int GetHashCode(MultivariatePolynomial<T> obj)
		{
			return obj.GetHashCode();
		}

		/// <summary>
		/// Returns a hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
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

		/// <summary>
		/// Converts the polynomial of the current instance to its equivalent string representation.
		/// </summary>
		public override string ToString()
		{
			string signString = string.Empty;
			string termString = string.Empty;
			string result = string.Empty;

			result = string.Join(" + ", Terms.Select(trm => trm.ToString()));
			result = result.Replace(" + -", " - ");

			return result;
		}

		#endregion

	}
}
